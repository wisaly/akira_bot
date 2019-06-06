using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PSplatoonMap : IMessageProcess
    {
        Schedule schedule_ = new Schedule();
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent msgevt, long srcid)
        {
            if (Global.msgFilter(msgevt.message, true, s => s.Contains("图") || s.Contains("工")))
            {
                Message msg = new Message();
                foreach (var ele in msgevt.message.data)
                {
                    if (ele is ElementAt)
                    {
                        ElementAt eat = ele as ElementAt;
                        if (eat.data["qq"] == Global.botid.ToString())
                        {
                            msg.data.Add(new ElementAt(Global.sbid));
                            continue;
                        }
                    }

                    msg.data.Add(ele);
                }

                await client.SendMessageAsync(msgevt.messageType, srcid, msg);
                return true;
            }

            if (Global.msgFilter(msgevt.message, true, s => s.Contains("下次区域") || s.Contains("下次鱼") || s.Contains("下次蛤") || s.Contains("下次塔")))
            {
                string text = schedule_.nextMode(Global.msgText(msgevt.message));

                if (text == "")
                    await client.SendMessageAsync(msgevt.messageType, srcid, new Message(
                        new ElementAt(msgevt.sender.user_id),
                        new ElementText(" 最近没有呢")));
                else
                    await client.SendMessageAsync(msgevt.messageType, srcid, new Message(
                        new ElementAt(Global.sbid),
                        new ElementText(" " + text)));

                return true;
            }

            return false;
        }
    }
}
