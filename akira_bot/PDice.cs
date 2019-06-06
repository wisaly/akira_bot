using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PDice : IMessageProcess
    {
        Random rand_ = new Random();
        WeaponDraw wp_ = new WeaponDraw();
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string keyword = "抽签";

            //if (me.message.data.Count > 0 && me.message.data[0] is ElementText)
            //{
            //    ElementText ele = me.message.data[0] as ElementText;
            //    if (ele.text.ToLower().Contains('d'))
            //    {
            //        try
            //        {
            //            string s = ele.text.ToLower().Trim();
            //            string[] sa = s.Split('d', options: StringSplitOptions.RemoveEmptyEntries);
            //            if (sa.Length == 2)
            //            {
            //                int cnt = int.Parse(sa[0]);
            //                int rng = int.Parse(sa[1]);
            //                if (cnt > 0 && cnt < 100 && rng > 0)
            //                {
            //                    int[] result = new int[cnt];
            //                    for (int i = 0; i < cnt; i++)
            //                        result[i] = rand_.Next(rng) + 1;
            //                    string ans = string.Join("，", result);

            //                    await client.SendMessageAsync(me.messageType, srcid, new Message(
            //                        $"{me.sender.nickname}转动{cnt}个{rng}面骰子，掷出了{ans}。"));
            //                }
            //            }
            //        }
            //        catch
            //        {
            //        }
            //    }
            //}
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, isGroup, s => s.TrimStart().StartsWith(keyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        string txt = (ele as ElementText).text.TrimStart();
                        if (!txt.StartsWith(keyword))
                            continue;

                        txt = txt.Replace("（", "(").Replace("）", ")").Replace("，", ",");

                        int bi = txt.IndexOf("(") + 1;
                        int ei = txt.IndexOf(")");
                        if (bi == -1 || bi == txt.Length)
                            continue;

                        txt = ei != -1 ? txt.Substring(bi, ei - bi) : txt.Substring(bi);
                        string[] choices = txt.Split(',');

                        string result = choices[rand_.Next(choices.Length)];

                        await client.SendMessageAsync(me.messageType, srcid, new cqhttp.Cyan.Messages.Message(
                            new ElementAt(me.sender.user_id), new ElementText($" 抽到了 {result} 呢！")));

                        break;
                    }
                }

                return true;
            }
            else if (Global.msgFilter(me.message, true, s=> s.Contains("抽很多武器")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new cqhttp.Cyan.Messages.Message(
                    ".随机武器"));
                return true;
            }
            else if (Global.msgFilter(me.message, isGroup, s => s.Contains("抽武器")))
            {
                var img = /*me.sender.user_id == Global.kusuriid ? wp_.generate4K() : */wp_.generate();
                MemoryStream ms = new MemoryStream();
                img.Item1.Save(ms, ImageFormat.Jpeg);

                await client.SendMessageAsync(me.messageType, srcid, new cqhttp.Cyan.Messages.Message(
                    new ElementAt(me.sender.user_id),
                    new ElementText($" 抽到 {img.Item2} 了呢"),
                    new ElementImage(ms.ToArray())));
                return true;
            }

            return false;
        }
    }
}
