using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace akira_bot
{
    class TencentAI : IAI
    {
        const string appid_ = "2116564033";
        const string appkey_ = "zzyPg3XRhGbz2H5n";
        MD5 md5_ = MD5.Create();
        Random rand_ = new Random();
        string session_ = "1001";
        public TencentAI()
        {
            session_ = noncestr();
        }

        string paramstr(Param[] param)
        {
            string parastr = "";
            Regex reg = new Regex(@"%[a-f0-9]{2}");
            foreach (var p in param)
            {
                if (p.v != "")
                {
                    string lower = HttpUtility.UrlEncode(p.v);
                    string upper = reg.Replace(lower, m => m.Value.ToUpperInvariant());
                    parastr += p.k + "=" + upper + "&";
                }
            }
            return parastr;
        }
        string sign(Param[] param, string appkey)
        {
            Array.Sort(param, (v1, v2) => v1.k.CompareTo(v2.k));
            string parastr = paramstr(param);
            
            parastr += "app_key=" + appkey;

            byte[] data = md5_.ComputeHash(Encoding.UTF8.GetBytes(parastr));
            var sb = new StringBuilder();
            foreach (byte t in data)
                sb.Append(t.ToString("X2"));
            
            return sb.ToString();
        }

        string timestamp()
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds).ToString();
        }

        string noncestr()
        {
            return rand_.Next().ToString();
        }

        public string chat(string text)
        {
            Param[] reqParam = new Param[]
            {
                new Param {k="app_id",v =appid_},
                new Param {k="time_stamp", v=timestamp()},
                new Param {k="nonce_str", v = noncestr()},
                new Param {k="session", v=session_},
                new Param {k="question", v=text},
                new Param {k="sign", v=""}
            };

            string signval = sign(reqParam, appkey_);

            foreach (var p in reqParam)
            {
                if (p.k == "sign")
                {
                    p.v = signval;
                    break;
                }
            }

            string parastr = paramstr(reqParam).TrimEnd('&');

            WebClient wc = new WebClient();
            string response = wc.DownloadString("https://api.ai.qq.com/fcgi-bin/nlp/nlp_textchat?" + parastr);
            JObject jores = JObject.Parse(response);
            if ((int)jores["ret"] != 0)
            {
                Console.WriteLine($"Tencent ai ret {response}");
                return "";
            }
            return jores["data"]["answer"].ToString();
        }
    }
    class Param
    {
        public string k;
        public string v;
    }
}
