using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cqhttp.Cyan;
using cqhttp.Cyan.Enums;
using cqhttp.Cyan.Events.CQEvents;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Events.CQResponses;
using cqhttp.Cyan.Events.CQResponses.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class Program
    {
        static void Main(string[] args)
        {
            //Logger.LogLevel = Verbosity.ALL;
            CQApiClient client = new CQHTTPClient(
                accessUrl: "http://127.0.0.1:5700",
                listen_port: 9000
                //accessToken: "akiratoken"
            );
            //CQApiClient client = new CQWebsocketClient(
            //    accessUrl: "ws://127.0.0.1:6700/api/",
            //    eventUrl: "ws://127.0.0.1:6700/events/");
            Console.WriteLine(
                $"QQ:{client.self_id},{client.self_nick}"
            );

            Global.botid = client.self_id;

            Global.processers.Add(new PCloudPPPP());
            Global.processers.Add(new PDice());
            Global.processers.Add(new PCloudDraw());
            Global.processers.Add(new PSplatoonMap());
            Global.processers.Add(new PGoldVerse());
            Global.processers.Add(new PSimpleQuestion());
            Global.processers.Add(new Trainable());
            Global.processers.Add(new AI());
            Global.processers.Add(new PLastProcesser());
            Global.processers.Add(new PRepeater());

            client.OnEventAsync += OnEventAsync;
            Console.ReadLine();
        }

        static async Task<CQResponse> OnEventAsync(CQApiClient client, CQEvent eventObj)
        {
            long srcid = -1;
            MessageEvent msgeve = null;

            if (eventObj is GroupMessageEvent)
            {
                var me = (eventObj as GroupMessageEvent);
                msgeve = me;
                srcid = me.group_id;
            }
            else if (eventObj is PrivateMessageEvent)
            {
                var me = eventObj as PrivateMessageEvent;
                msgeve = me;
                srcid = me.sender_id;
            }

            if (msgeve != null && Global.enabled_)
            {
                try
                {
                    foreach (var prcs in Global.processers)
                    {
                        if (await prcs.ProcessAsync(client, msgeve, srcid))
                            break;
                    }
                }
                catch (Exception e)
                {
                    await client.SendMessageAsync(msgeve.messageType, srcid, new Message(
                        new ElementAt(Global.masterid),
                        new ElementText($"我崩了:{e.Message}")));
                }
            }
            
            return new EmptyResponse();
        }
    }
}