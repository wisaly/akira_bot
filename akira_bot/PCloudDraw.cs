using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using cqhttp.Cyan.Events.CQEvents.Base;
using cqhttp.Cyan.Instance;
using cqhttp.Cyan.Messages.CQElements;

namespace akira_bot
{
    class PCloudDraw : IMessageProcess
    {
        GearDraw gd = new GearDraw();

        async public Task<bool> ProcessAsync(CQApiClient client, MessageEvent me, long srdid)
        {
            bool cloud = false;
            bool includeLocked = false;
            bool isGroup = me is cqhttp.Cyan.Events.CQEvents.GroupMessageEvent;
            if (Global.msgFilter(me.message, isGroup, (s) => 
                s.Contains("云抽奖") || s.Contains("雲菗奬") || s.Contains("抽云奖") || s.Contains("抽奖云")))
            {
                cloud = true;
            }
            else if (Global.msgFilter(me.message,true, s=> s.Contains("非酋检定")))
            {
                includeLocked = true;
            }
            else if (!Global.msgFilter(me.message,true,(s)=>s.Contains("抽奖")))
            {
                return false;
            }

            if (me.sender.user_id == Global.koiid)
                cloud = true;
            if (me.sender.user_id == Global.kusuriid)
                includeLocked = true;

            Image img = gd.Generate(cloud,includeLocked);
            MemoryStream ms = new MemoryStream();
            img.Save(ms, ImageFormat.Jpeg);

            await client.SendMessageAsync(me.messageType, srdid, new cqhttp.Cyan.Messages.Message(
                new ElementAt(me.sender.user_id),
                new ElementImage(ms.ToArray())));
            
            return true;
        }

        //string[] pool_ = new string[]
        //{
        //    "主省","副省","回墨","主强","减伤","安全鞋","复活","超级跳","走速","游速","副强","SP充能","SP强化","SP减少量DOWN"
        //};
        //string[] uniquePool_ = new string[]
        //{
        //    "隐跳","受身术","对物UP","复活强化","开局冲刺","结束冲刺","逆境强化","复仇","隐游","死惩","热感墨水"
        //};

    }
}
