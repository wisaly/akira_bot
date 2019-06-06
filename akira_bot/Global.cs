using cqhttp.Cyan.Messages;
using cqhttp.Cyan.Messages.CQElements;
using System;
using System.Collections.Generic;
using System.Text;

namespace akira_bot
{
    class Global
    {
        public static long botid = 0;
        public static long sbid = 1969899901;
        public static long masterid = 596830047;
        public static long koiid = 492216494;
        public static long kusuriid = 3432048410;

        public static string appDir = System.IO.Directory.GetCurrentDirectory();

        public static bool enabled_ = true;

        public static List<IMessageProcess> processers = new List<IMessageProcess>();

        public static bool msgFilter(Message msg, bool filterAt, Func<string, bool> filterText)
        {
            bool foundAt = false;
            bool foundText = false;
            foreach (var ele in msg.data)
            {
                if (filterAt && ele is ElementAt)
                {
                    ElementAt ea = ele as ElementAt;
                    if (ea.data["qq"] == Global.botid.ToString())
                        foundAt = true;
                }
                if (ele is ElementText)
                {
                    ElementText et = ele as ElementText;
                    if (filterText(et.text))
                        foundText = true;
                }
            }
            return (filterAt ? foundAt : true) && foundText;
        }

        public static string msgText(Message msg)
        {
            string txt = "";
            foreach (var ele in msg.data)
            {
                if (ele is ElementText)
                {
                    ElementText et = ele as ElementText;
                    txt += et.text.Trim();
                }
            }

            return txt;
        }

        static Random rand_ = new Random();
        public static T randItem<T>(T[] candidates)
        {
            return candidates[rand_.Next(candidates.Length)];
        }
    }
}
