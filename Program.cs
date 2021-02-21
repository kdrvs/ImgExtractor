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

            await (new ImageSearcher(dir.FilePathsList,
                new FileType()
                {
                    FileExtension = "png",
                    Signatures = new string[] { "89", "50", "4E", "47" } // png signature
                }))
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
                new FileType()
                {
                    FileExtension = "jpg",
                    Signatures = new string[] { "FF", "D8", "FF", "E0" } // jpg signature 1
                }))
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
                new FileType()
                {
                    FileExtension = "jpg",
                    Signatures = new string[] { "FF", "D8", "FF", "DB" } // jpg signature 2
                })) 
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
                new FileType()
                {
                    FileExtension = "jpeg",
                    Signatures = new string[] {"FF", "D8", "FF", "E1"} // gpeg signature
                })) 
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
                new FileType() {
                    FileExtension = "gif",
                    Signatures = new string[] { "47", "49", "46", "38", "37", "61" } // gif signature 1
                })) 
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
                new FileType()
                {
                    FileExtension = "gif",
                    Signatures = new string[] { "47", "49", "46", "38", "39", "61" } // gif signature 2
                 }))
                .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "tif",
                   Signatures = new string[] { "49", "49", "2A", "00" } // tif signature
                }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "tiff",
                   Signatures = new string[] { "4D", "4D", "00", "2A" } // tiff signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "zip",
                   Signatures = new string[] { "50", "4B", "03", "04", } // zip signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "pdf",
                   Signatures = new string[] { "25", "50", "44", "46" } // pdf signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "7z",
                   Signatures = new string[] { "37", "7A", "BC", "AF", "27", "1C" } // 7z signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "webm",
                   Signatures = new string[] { "1A", "45", "DF", "A3" } // webm signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "rtf",
                   Signatures = new string[] { "7B", "5C", "72", "74", "66", "31" } // rtf signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "mpg",
                   Signatures = new string[] { "00", "00", "01", "BA" } // mpg signature
               }))
               .copyImagesAsync();

            await (new ImageSearcher(dir.FilePathsList,
               new FileType()
               {
                   FileExtension = "mpeg",
                   Signatures = new string[] { "00", "00", "01", "B3" } // mpeg signature
               }))
               .copyImagesAsync();


            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Directory: " + Directory.GetCurrentDirectory().ToString());
            Console.ResetColor();
        }
    }
}
