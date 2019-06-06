using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace akira_bot
{
    class TulingAI : IAI
    {
        WebClient wc = new WebClient();
        public TulingAI()
        {
            wc.Encoding = Encoding.UTF8;
        }

        public string chat(string ques)
        {
            //dynamic jo = new JObject();
            //jo.reqType = 0;
            //jo.perception = new JObject();
            //jo.perception.inputText = new JObject();
            //jo.perception.inputText.text = ques;
            //jo.userInfo = new JObject();
            //jo.userInfo.apiKey = "d23c095617214de382565166058a727b";
            //jo.userInfo.userId = "akirabot";

            //string json = JsonConvert.SerializeObject(jo);

            try
            {
                var jo = new JObject();
                jo["reqType"] = 0;
                jo["perception"] = new JObject();
                jo["perception"]["inputText"] = new JObject();
                jo["perception"]["inputText"]["text"] = ques;
                jo["userInfo"] = new JObject();
                jo["userInfo"]["apiKey"] = "d23c095617214de382565166058a727b";
                jo["userInfo"]["userId"] = "akirabot";

                string json = jo.ToString();
                string response = wc.UploadString("http://openapi.tuling123.com/openapi/api/v2", json);
                var jores = JObject.Parse(response);
                string ans = jores["results"][0]["values"]["text"].ToString();
                return ans;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
                return "";
            }
        }
    }
}
