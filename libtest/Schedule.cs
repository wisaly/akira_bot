using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace libtest
{
    class Schedule
    {
        string url_ = "https://splatoon2.ink/data/schedules.json";
        WebClient wc_ = new WebClient();

        JObject download()
        {
            string json = wc_.DownloadString(url_);
            return JObject.Parse(json);
        }

        DateTime getTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(timestamp)
                .ToLocalTime();
        }

        public string nextMode(string mode)
        {
            Dictionary<string, string> modeTrans = new Dictionary<string, string>()
            {
                { "区域", "splat_zones" },
                { "塔", "tower_control" },
                { "蛤", "clam_blitz" },
                { "蛤蜊", "clam_blitz" },
                { "鱼", "rainmaker" }
            };
            JObject jo = download();
            JArray gachi = jo["gachi"] as JArray;
            foreach (JObject item in gachi)
            {
                Console.WriteLine(item["rule"]["key"].Value<string>());
                Console.WriteLine(getTime(item["end_time"].Value<long>()));
                if (item["rule"]["key"].Value<string>() == modeTrans[mode] &&
                    getTime(item["end_time"].Value<long>()) > DateTime.Now)
                {
                    DateTime startTime = getTime(item["start_time"].Value<long>());
                    double hrs = (startTime - DateTime.Now).TotalHours;
                    string result = "";
                    if (hrs > 2)
                        result = new string('下', (int)hrs / 2);

                    return result + "图";
                }
            }

            return "";
        }
    }
}
