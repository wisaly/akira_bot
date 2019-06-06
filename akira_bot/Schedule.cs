using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace akira_bot
{
    class Schedule
    {
        string url_ = "https://splatoon2.ink/data/schedules.json";
        WebClient wc_ = new WebClient();
        JObject cache_ = null;
        DateTime cacheExpire_ = DateTime.MinValue;

        JObject download()
        {
            if (cache_ != null && DateTime.Now < cacheExpire_)
                return cache_;

            string json = wc_.DownloadString(url_);
            cache_ = JObject.Parse(json);
            cacheExpire_ = DateTime.Now.AddHours(2);
            return cache_;
        }

        DateTime getTime(long timestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(timestamp)
                .ToLocalTime();
        }

        public string nextMode(string mode)
        {
            string[] items = mode.Split("下次");
            int nextCnt = 0;
            foreach (var c in items[0])
                if (c == '下')
                    nextCnt++;

            mode = items[1];

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
            int foundCnt = 0;
            foreach (JObject item in gachi)
            {
                if (item["rule"]["key"].Value<string>() == modeTrans[mode] &&
                    getTime(item["end_time"].Value<long>()) > DateTime.Now)
                {
                    if (++foundCnt <= nextCnt)
                        continue;
                    DateTime startTime = getTime(item["start_time"].Value<long>());
                    double hrs = (startTime - DateTime.Now).TotalHours;
                    string result = "";
                    if (hrs > 0)
                        result = new string('下', (int)hrs / 2 + 1);

                    return result + "图";
                }
            }

            return "";
        }
    }
}
