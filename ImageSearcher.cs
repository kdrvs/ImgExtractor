using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImgExtractor
{
    public class ImageSearcher
    {

        private string targetPNG = "Png";
        private string targetJPG = "Jpg";

        public List<string> Png { get; set; } = new List<string>();
        public List<string> Jpg { get; set; } = new List<string>();

        private List<string> PngSignatures = new List<string>()
                        {
                            "89", "50", "4E", "47"  // png Signature
                        };

        private List<string> JpgSignatures = new List<string>()
                        {
                            "FF", "D8", "FF", "DB"  // jpg Signature
                        };

        public ImageSearcher(List<string> paths)
        {
            imageSort(paths);
        }

        private void imageSort(List<string> paths)
        {
            foreach (string path in paths)
            {
                if (validator(path, PngSignatures))
                    Png.Add(path);
                else if (validator(path, JpgSignatures))
                    Jpg.Add(path);
            }

        }

        public void copyImages()
        {
            if (!Directory.Exists(targetPNG))
                Directory.CreateDirectory(targetPNG);
            if (!Directory.Exists(targetJPG))
                Directory.CreateDirectory(targetJPG);

            var index = 0;
            foreach (string _png in Png)
            {
                File.Copy(_png, targetPNG + "/" + index.ToString() + ".png");
                index++;
                
            }

            index = 0;
            foreach (string _jpg in Jpg)
            {
                File.Copy(_jpg, targetJPG + "/" + index.ToString() + ".jpg");
                index++;
            }

        }

        public async Task<bool> copyImagesAsync()
        {
            if (!Directory.Exists(targetPNG))
                Directory.CreateDirectory(targetPNG);
            if (!Directory.Exists(targetJPG))
                Directory.CreateDirectory(targetJPG);

            var index = 0;
            foreach (string _png in Png)
            {
                using (Stream source = File.Open(_png, FileMode.Open))
                {
                    using (Stream target = File.Create(targetPNG + "/" + index.ToString() + ".png"))
                    {
                        await source.CopyToAsync(target);
                        index++;
                    }
                }
                
            }

            index = 0;
            foreach (string _jpg in Jpg)
            {
                using (Stream source = File.Open(_jpg, FileMode.OpenOrCreate))
                {
                    using (Stream target = File.Create(targetJPG + "/" + index.ToString() + ".jpg"))
                    {
                        await source.CopyToAsync(target);
                        index++;
                    }
                }
       
            }

            return true;

        }

        private bool validator(string path, List<string> signatures)
        {
            if (!File.Exists(path))
                return false;

            var fileHeader = new List<string>();
            using FileStream stream = File.OpenRead(path);
            for (int i = 0; i < 4; i++)
            {
                fileHeader.Add(stream.ReadByte().ToString("X2"));
            }

            if (fileHeader.Intersect(signatures).Any())
                return true;

            return false;

        }





    }
}