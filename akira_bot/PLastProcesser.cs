using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using cqhttp.Cyan.Messages.CQElements.Base;

namespace akira_bot
{
    class PLastProcesser : IMessageProcess
    {
        byte[] imgConfuse_ = null;
        public PLastProcesser()
        {
            imgConfuse_ = File.ReadAllBytes(Path.Combine(Global.appDir, "image", "confuse.gif"));
        }

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (!Global.msgFilter(me.message, isGroup, s=>true))
                return false;
            if (me.sender.user_id == Global.sbid)
                return false;

            await client.SendMessageAsync(me.messageType, srcid, new Message(
                new ElementImage(imgConfuse_)));
            return true;
        }
    }
}
