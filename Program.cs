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

            string sourcePath = "";
            string targetPath = "files";
            
            if(args == null || args.Length < 1)
            {
                Console.WriteLine("Insert a folder with files : ");
                sourcePath = Console.ReadLine().Replace(" ", "").Replace("\"","").Replace("\'", "");
            }
            else
            {
                try
                {
                    if(args[0] != null)
                    sourcePath = args[0];

                    if(args.Length > 1)
                        if(args[1] != null)
                            targetPath = args[1].Replace(" ", "").Replace("\"","").Replace("\'", "") + "/" + targetPath;
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                
                
            }

            
            try
            {
                var dir = new Paths(sourcePath);
                await start(dir.FilePathsList, targetPath);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Files saved to :" + (new DirectoryInfo(targetPath)).FullName + "\n");
                Console.ResetColor();
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public static async Task start(List<string> dirs, string targetFolder)
        {
            try
            {
                List<FileType> fileTypes = await Signatures.getTypeListAsync();
                var map = new FileMapper(dirs, fileTypes, targetFolder);
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
                    Console.WriteLine($"Found of {sum.Key}:\t{sum.Value}");
                }
            }

        
    }
}
