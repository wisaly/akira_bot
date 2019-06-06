using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace akira_bot
{
    class WeaponDraw
    {
        Random rand_ = new Random();

        string s2datapath_ = Path.Combine(Global.appDir, "s2data");
        List<Weapon> weapons_ = new List<Weapon>();
        List<Weapon> weapon4k_ = new List<Weapon>();
        Image bk_ = null;
        public WeaponDraw()
        {
            StreamReader file = File.OpenText(Path.Combine(s2datapath_, "splatoon2-data.json"));
            JsonTextReader reader = new JsonTextReader(file);

            JObject jo = (JObject)JToken.ReadFrom(reader);
            foreach (var jw in jo["weapons"])
            {
                Weapon weapon = new Weapon
                {
                    name = jw["name"].ToString(),
                    image = jw["image"].ToString(),
                    special = jw["special"]["image_a"].ToString(),
                    sub = jw["sub"]["image_a"].ToString()
                };
                if (weapon.name.Contains("4K"))
                    weapon4k_.Add(weapon);
                weapons_.Add(weapon);
            }
            bk_ = Image.FromFile(Path.Combine(s2datapath_, "weapon-bk.png"));
        }

        (Image, string) drawImage(Weapon wp)
        {
            Image result = bk_.Clone() as Image;
            Graphics g = Graphics.FromImage(result);
            g.DrawImage(Image.FromFile(s2datapath_ + wp.image), 60, 20, 190, 190);
            g.DrawImage(Image.FromFile(s2datapath_ + wp.sub), 68, 220, 64, 64);
            g.DrawImage(Image.FromFile(s2datapath_ + wp.special), 186, 220, 64, 64);
            return (result, wp.name);
        }
        public (Image,string) generate()
        {
            Weapon wp = weapons_[rand_.Next(weapons_.Count)];
            return drawImage(wp);
        }

        public (Image, string) generate4K()
        {
            Weapon wp = weapon4k_[rand_.Next(weapon4k_.Count)];
            return drawImage(wp);
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
