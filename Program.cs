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
            var map = new FileMapper(dirs, fileTypes);
            await map.searchFilesAsync();
            printMap(map.getAmountMap());
            await map.copyAsync();
        }

        public static void printMap (Dictionary<string, int> map)
            {
                foreach(KeyValuePair<string, int> sum in map)
                {
                    Console.WriteLine($"Found of {sum.Key} files: {sum.Value}");
                }
            }

        
    }
}
