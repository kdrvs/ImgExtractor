using System;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using System.IO;

namespace ImgExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {

            string path;

            Console.WriteLine("Insert folder with files : ");
            path = Console.ReadLine();
            var dir = new Paths(path);

            await run(dir.FilePathsList);

            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Directory: " + Directory.GetCurrentDirectory().ToString());
            Console.ResetColor();
        }

        public static async Task run(List<string> dirs)
        {
            await(new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "png",
                    Signatures = new string[] { "89", "50", "4E", "47" } // png signature
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "jpg",
                    Signatures = new string[] { "FF", "D8", "FF", "E0" } // jpg signature 1
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "jpg",
                    Signatures = new string[] { "FF", "D8", "FF", "DB" } // jpg signature 2
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "jpeg",
                    Signatures = new string[] { "FF", "D8", "FF", "E1" } // gpeg signature
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "gif",
                    Signatures = new string[] { "47", "49", "46", "38", "37", "61" } // gif signature 1
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
                new FileType()
                {
                    FileExtension = "gif",
                    Signatures = new string[] { "47", "49", "46", "38", "39", "61" } // gif signature 2
                }))
                .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "tif",
                   Signatures = new string[] { "49", "49", "2A", "00" } // tif signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "tiff",
                   Signatures = new string[] { "4D", "4D", "00", "2A" } // tiff signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "zip",
                   Signatures = new string[] { "50", "4B", "03", "04", } // zip signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "pdf",
                   Signatures = new string[] { "25", "50", "44", "46" } // pdf signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "7z",
                   Signatures = new string[] { "37", "7A", "BC", "AF", "27", "1C" } // 7z signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "webm",
                   Signatures = new string[] { "1A", "45", "DF", "A3" } // webm signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "rtf",
                   Signatures = new string[] { "7B", "5C", "72", "74", "66", "31" } // rtf signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "mpg",
                   Signatures = new string[] { "00", "00", "01", "BA" } // mpg signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   FileExtension = "mpeg",
                   Signatures = new string[] { "00", "00", "01", "B3" } // mpeg signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "m4a",
                   Signatures = new string[] { "66", "74", "79", "70", "4D", "34", "41", "20" } // m4a signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "flv",
                   Signatures = new string[] { "66", "74", "79", "70", "4D", "34", "56", "20" } // flv signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "mp4",
                   Signatures = new string[] { "66", "74", "79", "70", "4D", "53", "4E", "56" } // mp4 signature
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "mp4",
                   Signatures = new string[] { "66", "74", "79", "70", "69", "73", "6F", "6D" } // mp4 signature 2
                }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "m4v",
                   Signatures = new string[] { "66", "74", "79", "70", "6D", "70", "34", "32" } // m4v signature 2
                }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "mov",
                   Signatures = new string[] { "66", "74", "79", "70", "71", "74", "20", "20" } // mov signature 2
               }))
               .copyImagesAsync();

            await (new FileSearcher(dirs,
               new FileType()
               {
                   Offset = 4,
                   FileExtension = "mov",
                   Signatures = new string[] { "6D", "6F", "6F", "76" } // mov signature 2
               }))
               .copyImagesAsync();

        }
    }
}
