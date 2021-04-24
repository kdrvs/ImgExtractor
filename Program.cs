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
            
            try
            {
                var dir = new Paths(path);
                await start(dir.FilePathsList);
                Console.WriteLine("Files saved to :" + Directory.GetCurrentDirectory().ToString());
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static async Task start(List<string> dirs)
        {
            try
            {
                List<FileType> fileTypes = await Signatures.getTypeListAsync();
                var map = new FileMapper(dirs, fileTypes);
                await map.searchFilesAsync();
                printMap(map.getAmountMap());
                await map.copyAsync();
                //map.saveLog();
            }   
            catch(Exception E)
            {
                Console.WriteLine(E.Message);
            } 
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
