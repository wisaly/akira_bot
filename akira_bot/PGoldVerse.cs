using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PGoldVerse : IMessageProcess
    {
        GoldVerseJson gvj_ = new GoldVerseJson();
        Translator tr_ = new Translator();

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srcid)
        {
            const string trKeyword = "翻译";
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, isGroup, s => s.Contains("金句")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementAt(me.sender.user_id),
                    new ElementText(" " + gvj_.generate())));
                return true;
            }
            else if (Global.msgFilter(me.message, isGroup, s => s.Contains("签名") || s.Contains("個性佥洺")))
            {
                await client.SendMessageAsync(me.messageType, srcid, new Message(
                    new ElementAt(me.sender.user_id),
                    new ElementText(" " + tr_.generate())));
                return true;
            }
            else if (Global.msgFilter(me.message, isGroup, s => s.TrimStart().StartsWith(trKeyword)))
            {
                foreach (var ele in me.message.data)
                {
                    if (ele is ElementText)
                    {
                        string txt = (ele as ElementText).text.TrimStart();
                        if (!txt.StartsWith(trKeyword))
                            continue;

                        txt = txt.Substring(trKeyword.Length);
                        await client.SendMessageAsync(me.messageType, srcid, new Message(
                            new ElementAt(me.sender.user_id),
                            new ElementText(" " + tr_.translate(txt))));
                        return true;
                    }
                }
            }

            return false;
        }

    }
}
