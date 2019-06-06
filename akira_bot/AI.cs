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
    class AI : IMessageProcess
    {
        //IAI ai_ = new TulingAI();
        IAI ai_ = new TencentAI();
        //Translator tr_ = new Translator();
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, isGroup, s => true))
            {
                if (me.sender.user_id == Global.sbid)
                    return true;

                string ques = Global.msgText(me.message);
                string ans = ai_.chat(ques);
                if (ans == "")
                    return false;

                Message message = new Message();
                if (isGroup)
                    message.data.Add(new ElementAt(me.sender.user_id));
                message.data.Add(new ElementText(ans));
                await client.SendMessageAsync(me.messageType, srcid, message);

                return true;
            }

            return false;
        }
    }
}
