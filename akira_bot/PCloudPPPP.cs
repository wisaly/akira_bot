using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Enums;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PCloudPPPP : IMessageProcess
    {
        List<long> currentList_ = new List<long>();
        int expectCnt = 4;

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent msgevt, long srcid)
        {
            if (currentList_.Count > 0)
            {
                if (!Global.msgFilter(msgevt.message, false, (s) => s == "1"))
                    return false;

                if (currentList_.Contains(msgevt.sender.user_id))
                    return false;
                currentList_.Add(msgevt.sender.user_id);

                if (currentList_.Count == expectCnt)
                {
                    await client.SendMessageAsync(msgevt.messageType, srcid, new Message("比赛开始了！"));
                    await Task.Delay(TimeSpan.FromSeconds(5));

                    Message win = new Message();
                    for (int i = 0; i < expectCnt; i++)
                        win.data.Add(new ElementAt(currentList_[i]));
                    win.data.Add(new ElementText(" 取得了胜利！"));
                    await client.SendMessageAsync(msgevt.messageType, srcid, win);
                    await Task.Delay(TimeSpan.FromSeconds(2));

                    Message gold = new Message();
                    for (int i = 0; i < expectCnt; i++)
                        gold.data.Add(new ElementAt(currentList_[i]));
                    gold.data.Add(new ElementText(" 获得了金牌！"));
                    await client.SendMessageAsync(msgevt.messageType, srcid, gold);
                    currentList_.Clear();
                }
                else
                {

                    await client.SendMessageAsync(msgevt.messageType, srcid, new Message(
                        $"云{new string('排', expectCnt)} {currentList_.Count}q{expectCnt-currentList_.Count}"));
                }
                return true;
            }

            if (Global.msgFilter(msgevt.message, true, s =>
                 s.ToLower().Contains("云pppp") || s.Contains("云排排排排")))
            {
                expectCnt = 4;
            }
            else if (Global.msgFilter(msgevt.message, true, s =>
                 s.ToLower().Contains("云pp") || s.Contains("云排排")))
            {
                expectCnt = 2;
            }
            else
            {
                return false;
            }

            if (currentList_.Count == 0)
            {
                await client.SendMessageAsync(msgevt.messageType, srcid, new Message(
                    new ElementAt(msgevt.sender.user_id),
                    new ElementText($"发起了云{new string('排',expectCnt)} 加入的人请打1")));
                currentList_.Add(msgevt.sender.user_id);
            }

            return true;
        }
    }
}
