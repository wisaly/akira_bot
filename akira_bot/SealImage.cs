using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace akira_bot
{
    class SealImage
    {
        byte[][] seals_;
        public SealImage()
        {
            string dir = Path.Combine(Global.appDir, "image", "seal");
            FileInfo[] seals = new DirectoryInfo(dir).GetFiles();
            seals_ = new byte[seals.Length][];
            for (int i = 0; i < seals.Length; i++)
            {
                seals_[i] = File.ReadAllBytes(seals[i].FullName);
            }
        }

        public byte[] seal()
        {
            return Global.randItem(seals_);
        }
    }
}
