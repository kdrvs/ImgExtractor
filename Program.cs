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

            Console.WriteLine("Insert a folder with files : ");
            path = Console.ReadLine();
            var dir = new Paths(path);
            
            await start(dir.FilePathsList);
            Console.WriteLine("Directory :" + Directory.GetCurrentDirectory().ToString());
         
        }

        public static async Task start(List<string> dirs)
        {
            List<FileType> fileTypes = await Signatures.getTypeListAsync();
            foreach(FileType type in fileTypes)
            {
               await (await FileSearcher.CreateAsync(dirs, type)).copyImagesAsync();
            }
        }

        
    }
}
