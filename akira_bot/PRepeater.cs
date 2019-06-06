using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PRepeater : IMessageProcess
    {
        public PRepeater()
        {

        }

        string lastMsg_ = "";
        int otherRepeater_ = 0;
        Random rand_ = new Random();
        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            foreach (var ele in me.message.data)
            {
                if (ele is ElementAt)
                    return false;
            }

            if (me.message.ToString() == lastMsg_)
            {
                otherRepeater_++;
                if (otherRepeater_ > rand_.Next(3, 10))
                {
                    await client.SendMessageAsync(me.messageType, srcid, me.message);

                    otherRepeater_ = 0;
                    lastMsg_ = "";
                    return true;
                }
            }
            else
            {
                lastMsg_ = me.message.ToString();
                otherRepeater_ = 0;
            }

            return false;
        }
    }
}
