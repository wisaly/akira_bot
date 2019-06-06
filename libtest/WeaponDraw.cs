using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace libtest
{
    class WeaponDraw
    {
        Random rand_ = new Random();

        string s2datapath_ = Path.Combine(@"C:\home\project\akira_bot\akira_bot\bin\Debug\netcoreapp2.1", "s2data");
        List<Weapon> weapons_ = new List<Weapon>();
        public WeaponDraw()
        {
            StreamReader file = File.OpenText(Path.Combine(s2datapath_, "splatoon2-data.json"));
            JsonTextReader reader = new JsonTextReader(file);

            JObject jo = (JObject)JToken.ReadFrom(reader);
            foreach (var jw in jo["weapons"])
            {
                weapons_.Add(new Weapon
                {
                    name = jw["name"].ToString(),
                    image = jw["image"].ToString(),
                    special = jw["special"]["image_a"].ToString(),
                    sub = jw["sub"]["image_a"].ToString()
                });
            }
        }

        public Image generate()
        {
            Weapon wp = weapons_[rand_.Next(weapons_.Count)];
            Image result = new Bitmap(320, 256, PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(result);
            g.FillRectangle(Brushes.White, 0, 0, result.Width, result.Height);
            g.DrawImage(Image.FromFile(s2datapath_  + wp.image),0,0,256,256);
            g.DrawImage(Image.FromFile(s2datapath_ + wp.sub), 256, 96, 64, 64);
            g.DrawImage(Image.FromFile(s2datapath_ + wp.special), 256, 192, 64, 64);
            return result;
        }
    }

    class Weapon
    {
        public string name;
        public string image;
        public string special;
        public string sub;
    }
}
