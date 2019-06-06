using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Web;
using static HtmlAgilityPack.HtmlWeb;

namespace akira_bot
{
    class Translator
    {
        Random rand_ = new Random();
        HtmlWeb htmlweb_ = new HtmlWeb();
        public string generate()
        {
            HtmlDocument doc = htmlweb_.Load("http://m.fzlft.com/");

            return randOne(parseDoc(doc));
        }
        public string translate(string s)
        {
            PreRequestHandler handler = delegate (HttpWebRequest request) {
                string payload = "q=" + HttpUtility.UrlEncode(s);
                byte[] buff = Encoding.ASCII.GetBytes(payload.ToCharArray());
                request.ContentLength = buff.Length;
                request.ContentType = "application/x-www-form-urlencoded";
                System.IO.Stream reqStream = request.GetRequestStream();
                reqStream.Write(buff, 0, buff.Length);
                return true;
            };
            htmlweb_.PreRequest += handler;
            HtmlDocument doc = htmlweb_.Load("http://m.fzlft.com/?", "POST");
            htmlweb_.PreRequest -= handler;

            return randOne(parseDoc(doc));

        }
        string[] parseDoc(HtmlDocument doc)
        {
            string[] result = new string[3];
            for (int i = 0; i < 3; i++)
            {
                HtmlNode node = doc.DocumentNode.SelectSingleNode($"//*[@id=\"result{i}\"]");
                result[i] = HttpUtility.HtmlDecode(node.InnerText);
            }

            return result;
        }
        string randOne(string[] sa)
        {
            return sa[rand_.Next(sa.Length)];
        }
    }
}
