
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Text;

namespace akira_bot
{
    class GearDraw
    {
        Image[] regularImg_ = null;
        Image[] uniqueImg_ = null;
        Image bgImg_ = null;
        Image lockedImg_ = null;
        Random rand_ = new Random();

        public GearDraw()
        {
            string imgpath = Path.Combine(Directory.GetCurrentDirectory(), "image");
            DirectoryInfo regDir = new DirectoryInfo(Path.Combine(imgpath, "regular"));
            FileInfo[] rfi = regDir.GetFiles();
            regularImg_ = new Image[rfi.Length];
            for (int i = 0; i < rfi.Length; i++)
            {
                regularImg_[i] = Image.FromFile(rfi[i].FullName);
            }

            DirectoryInfo uniDir = new DirectoryInfo(Path.Combine(imgpath, "unique"));
            FileInfo[] ufi = uniDir.GetFiles();
            uniqueImg_ = new Image[ufi.Length];
            for (int i = 0; i < ufi.Length; i++)
            {
                uniqueImg_[i] = Image.FromFile(ufi[i].FullName);
            }

            bgImg_ = Image.FromFile(Path.Combine(imgpath, "bg.png"));
            lockedImg_ = Image.FromFile(Path.Combine(imgpath, "locked.png"));
        }

        bool lucky()
        {
            return rand_.Next(100) < 85;
        }
        bool unlucky()
        {
            return rand_.Next(100) < 77;
        }
        Image randFile(bool includeUnique, bool needluck)
        {
            bool luck = needluck && lucky();
            int i = luck && !includeUnique && lastRand_ != -1  && lastRand_ < regularImg_.Length?
                lastRand_ :
                rand_.Next(includeUnique && !luck ? 
                    regularImg_.Length + uniqueImg_.Length :
                    regularImg_.Length);
            lastRand_ = i;
            return i < regularImg_.Length ? regularImg_[i] : uniqueImg_[i - regularImg_.Length];
        }

        int lastRand_ = -1;
        public Image Generate(bool needluck, bool includeLocked = false)
        {
            lastRand_ = -1;

            Image imgm = randFile(true,needluck);
            Image[] imgs = new Image[3];
            for (int i = 0; i < 3; i++)
                imgs[i] = randFile(false,needluck);

            if (includeLocked)
            {
                for (int i = 2; i >= 0; i--)
                {
                    if (unlucky())
                        imgs[i] = lockedImg_;
                    else
                        break;
                }
            }
            
            Image img = bgImg_.Clone() as Image;

            Graphics g = Graphics.FromImage(img);
            g.DrawImage(imgm, 20, 20, 64, 64);
            for (int i = 0; i < 3; i++)//111 178 244
                g.DrawImage(imgs[i], 111 + 66 * i, 41, 48, 48);
            return img;
        }
    }
}
