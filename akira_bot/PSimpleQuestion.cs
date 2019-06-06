using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PSimpleQuestion : IMessageProcess
    {
        Random rand_ = new Random();
        SealImage si_ = new SealImage();
        bool testRand(int r = 50)
        {
            return rand_.Next(100) < r;
        }
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            if (Global.msgFilter(me.message, true, s => s.Trim().ToLower() == "hi"))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    "抽取随机装备：@bot {[云]抽奖|非酋检定}\r\n" +
                    "抽取随机武器：@bot 抽[很多]武器\r\n" +
                    "抽签：@bot 抽签([<第一项>][,<第二项>][...][)]\r\n"+
                    "生成帅气的签名（联网）：@bot 签名\r\n"+
                    "生成不那么帅气的签名（本地1500条随机抽取）：@bot 金句\r\n" +
                    "将文字转换为帅气的格式：@bot 翻译[内容]\r\n" +
                    "云排排排排：@bot {云排排[排排]|云pp[pp]}\r\n" +
                    "喷喷的模式与打工时间表（调用其它模块）：@bot [下]{图|工}\r\n" +
                    "复读机模式：触发条件未知\r\n" +
                    "（不区分大小写与全角半角标点）"));
                return true;
            }
            //if (Global.msgFilter(me.message,true,s=>s.ToLower().Contains("akira")))
            //{
            //    string ans;
            //    if (me.sender.user_id == Global.masterid)
            //        ans = "我问你，你是我的master吗";
            //    else
            //        ans = "akira是我的master";
            //    await client.SendMessageAsync(me.messageType, srcid, new cqhttp.Cyan.Messages.Message(ans));
            //    return true;
            //}

            if (Global.msgFilter(me.message,false,s=> s.Contains("我今晚一个人在家")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message("我们谁又不是呢"));
                return true;
            }
            if (Global.msgFilter(me.message, false, s => s.Contains("还行")) && testRand(30))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message("还行"));
                return true;
            }
            if (Global.msgFilter(me.message, false, s => s.TrimEnd().EndsWith("呢")) && testRand(10))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message("是的呢"));
                return true;
            }
            if (Global.msgFilter(me.message, false, s => s.Contains("涂地吗")) && testRand(30))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementImage(File.ReadAllBytes(Path.Combine(Global.appDir, "image", "nawabari.jpg")))));
                return true;
            }
            if (Global.msgFilter(me.message, false, s => s.Contains("排排吗")) && testRand(30))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementImage(File.ReadAllBytes(Path.Combine(Global.appDir, "image", "pp.jpg")))));
                return true;
            }
            if (Global.msgFilter(me.message, true, s => s.Contains("你真棒") || s.Contains("你好棒")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementImage(File.ReadAllBytes(Path.Combine(Global.appDir, "image", "emoshy.jpg")))));
                return true;
            }
            if (me.sender.user_id == Global.masterid && Global.msgFilter(me.message, true, s => s.Contains("闭嘴")))
            {
                Global.enabled_ = false;
                await Task.Delay(TimeSpan.FromMinutes(1));
                Global.enabled_ = true;
                return true;
            }

            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, isGroup, s => s.TrimStart().StartsWith("你叫")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    Global.msgText(me.message).Replace("你叫", "我叫")));
                return true;
            }
            if (Global.msgFilter(me.message, isGroup, s => s.ToLower().Contains("koi今天论文写完了吗")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    Global.randItem(new string[] { "并没有~", "没有", "没", "Nope" })));
                return true;
            }
            if (Global.msgFilter(me.message, isGroup, s => s.ToLower().Contains("akira区域上s了吗")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    Global.randItem(new string[] { "没有", "还没有", "还是A-", "还是A-，鱼也是A-" })));
                return true;
            }
            if (/*(me.sender.user_id == Global.koiid || me.sender.user_id == Global.masterid) &&*/
                Global.msgFilter(me.message, isGroup, s => (s.Contains("戳") || s.Contains("摸") || s.Contains("蹭")) && s.Length < 5))
            {
                if (testRand(10))
                    await client.SendMessageAsync(me.messageType, srcid, new Message(
                        new ElementImage(si_.seal())));
                else
                    await client.SendMessageAsync(me.messageType, srcid, new Message(
                        Global.randItem(new string[] { "呜……", "啊！", "啊~", "（噗）", "（噗呲）", "嗷！"/*, "こい"*/, "emmmmm", "嗯……", "嘤！", "嘤……", "rua！" })));
                return true;
            }
            if (Global.msgFilter(me.message, isGroup, s => s.Contains("噗") && s.Length < 5))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    Global.randItem(new string[] { "戳", "戳~" })));
                return true;
            }
            if (Global.msgFilter(me.message, isGroup, s => s.Trim() == "草" || s.Trim() == "?" || s.Trim() == "？"))
            {
                return true;
            }
            if (me.sender.user_id != Global.koiid && testRand(1) && Global.msgFilter(me.message, isGroup, s=> s.Trim() == "在吗"))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    "buzai,cnm"));
                return true;
            }
            if (me.sender.user_id == Global.kusuriid && testRand(1) &&
                Global.msgFilter(me.message, isGroup, s=>true))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementImage(File.ReadAllBytes(Path.Combine(Global.appDir, "image", "emorefuse.jpg")))));
                return true;
            }

            return false;
        }
    }
}
