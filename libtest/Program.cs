using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Web;
using HtmlAgilityPack;
using iBoxDB.LocalServer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static HtmlAgilityPack.HtmlWeb;

namespace libtest
{
    class Program
    {
        static void Main(string[] args)
        {
            //iBoxDB.LocalServer.DB.Root(Path.Combine(Directory.GetCurrentDirectory(),"db"));


            //var db = new DB();
            //db.GetConfig().EnsureTable<qa>("k", "q");
            //AutoBox auto = db.Open();

            //string key = "啊嗷嗷嗷嗷嗷嗷嗷嗷";
            //bool b = key.StartsWith("啊嗷嗷嗷嗷嗷嗷嗷嗷");
            //string val = "呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜呜";
            ////auto.Insert("k", new qa {q = key, a =  });
            ////var o2 = auto.SelectKey("k", key);
            ////auto.Update("k", o1);
            ////auto.Delete("k", key);

            //KvTable kvt = new KvTable("qa");
            //kvt.insertOrUpdate(new Kv(key, val));

            //Kv kv2 = kvt.fuzzyQuery("嗷");
            //long x = kvt.count();

            //TestRand test = new TestRand();
            //test.run();

            string s = @"C:\home\temp\竖琴海豹";
            foreach (var fi in new DirectoryInfo(s).GetFiles())
            {
                if (fi.Name.Contains('@'))
                {
                    fi.MoveTo(fi.Name.Substring(0, fi.Name.IndexOf('@')));
                }
            }
        }

        private static void dumpgv()
        {
            JArray ja = new JArray();
            HtmlWeb htmlweb = new HtmlWeb();
            int cnt = 1;
            for (int i = 1; i <= 51; i++)
            {
                Console.WriteLine(i);
                HtmlDocument htmldoc = htmlweb.Load($"http://www.qqgexingqianming.com/huoxingwen/{i}.htm");
                HtmlNode nodeList = htmldoc.DocumentNode.SelectSingleNode("//*[@id=\"list1\"]");
                foreach (var nodeLi in nodeList.ChildNodes)
                {
                    if (nodeLi.Name != "li")
                        continue;
                    dynamic v = new JObject();
                    v.id = cnt;
                    v.text = nodeLi.FirstChild.InnerText;
                    ja.Add(v);
                    cnt++;
                }
                Thread.Sleep(300);
            }

            string json = JsonConvert.SerializeObject(ja);

            FileStream fs = new FileStream("gold_verse.json", FileMode.OpenOrCreate);
            StreamWriter sw = new StreamWriter(fs);
            sw.Write(json);
            sw.Close();
            fs.Close();
        }

        class qa
        {
            public string q;
            public string a;
        }
    }
}
