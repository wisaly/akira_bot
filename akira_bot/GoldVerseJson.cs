using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace akira_bot
{
    class GoldVerseJson
    {
        string[] gv_ = null;
        Random rand_ = new Random();
        public GoldVerseJson()
        {
            StreamReader file = File.OpenText(Path.Combine(Global.appDir, "gold_verse.json"));
            JsonTextReader reader = new JsonTextReader(file);

            JArray jo = (JArray)JToken.ReadFrom(reader);
            gv_ = new string[jo.Count];
            for (int i = 0; i < gv_.Length; i++)
            {
                gv_[i] = jo[i]["text"].ToString();
            }
        }
        public string generate()
        {
            return gv_[rand_.Next(gv_.Length)];
        }
    }
}
