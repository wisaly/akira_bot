using cqhttp.Cyan.Enums;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace akira_bot
{
    interface IMessageProcess
    {
        Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid);
    }
}
