using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace akira_bot
{
    class Trainable : IMessageProcess
    {
        Random rand_ = new Random();
        KvTable kvt_ = new KvTable("knowlage");
        const string keyword = "定义";

        public Trainable()
        { }

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            if (Global.msgFilter(me.message, true, s=>s.TrimStart().StartsWith(keyword)))
            {
                Kv kv = await processDefine(me.message, me.sender);
                if (kv == null)
                {
                    await client.SendMessageAsync(me.messageType, srcid, new Message(
                        new ElementAt(me.sender.user_id),
                        new ElementText(" 识别不了定义呢")));
                    return true;
                }

                try
                {
                    kvt_.insertOrUpdate(kv);

                    await client.SendMessageAsync(me.messageType, srcid, new Message(
                        new ElementAt(me.sender.user_id),
                        new ElementText($" 学会 {kv.k} 了！")));
                }
                catch (Exception e)
                {
                    await client.SendMessageAsync(me.messageType, srcid, new Message(
                        new ElementAt(Global.masterid),
                        new ElementText($" 数据库插不进去:{e.Message}")));
                }
                return true;
            }

            if (!Global.msgFilter(me.message, true, s => true))
                return false;

            Kv ans = processQuestion(Global.msgText(me.message));
            if (ans == null)
                return false;

            Tag tag = JsonConvert.DeserializeObject<Tag>(ans.r);
            if (tag.anstype == Tag.ansenum.text)
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementAt(me.sender.user_id),
                    new ElementText(" " + ans.v)));
                return true;
            }
            else if (tag.anstype == Tag.ansenum.image)
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementAt(me.sender.user_id),
                    new ElementImage(ans.v)));
                return true;
            }

            return false;
        }

        async Task<Kv> processDefine(Message msg, Sender sender)
        {
            string ques = "";
            string ans = "";
            string ansimg = "";
            foreach (var ele in msg.data)
            {
                if (ele is ElementText)
                {
                    ElementText elet = ele as ElementText;
                    if (!elet.text.TrimStart().StartsWith(keyword))
                        continue;
                    string defstr = elet.text.Trim().Substring(keyword.Length).Replace("：",":");
                    string[] defarr = defstr.Split(':', options: StringSplitOptions.RemoveEmptyEntries);

                    if (defarr.Length == 0)
                        return null;

                    ques = defarr[0];

                    if (ques.Contains(keyword))
                        return null;

                    if (defstr.EndsWith(':') && defarr.Length == 1)
                    {
                        // define image
                        continue;
                    }
                    if (defarr.Length != 2)
                        return null;

                    ans = defarr[1];
                    break;
                }
                if (ele is ElementImage && ques != "")
                {
                    ElementImage elei = ele as ElementImage;
                    if (!await elei.Fix())
                        return null;
                    ansimg = elei.data["file"];
                    break;
                }
            }
            if (ques == "" || (ans == "" && ansimg == ""))
                return null;

            Tag tag = new Tag
            {
                anstype = ans != "" ? Tag.ansenum.text : Tag.ansenum.image,
                trainerid = sender.user_id,
                trainername = sender.nickname
            };

            Kv kv = new Kv
            {
                k = ques,
                v = ans != "" ? ans : ansimg,
                r = JsonConvert.SerializeObject(tag)
            };
            return kv;
        }

        Kv processQuestion(string ques)
        {
            if (ques == "")
                return null;
            List<Kv> kvt = kvt_.fuzzyQuery(ques);
            if (kvt.Count == 0)
                return null;
            return kvt[rand_.Next(kvt.Count)];
        }

        class Tag
        {
            public enum ansenum { text, image };
            public ansenum anstype = ansenum.text;
            public long trainerid = 0;
            public string trainername = "";
            public DateTime updatedate = DateTime.Now;
        }
    }

}
