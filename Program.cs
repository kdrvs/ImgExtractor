using System;
using System.Threading.Tasks;
using System.Threading;

namespace ImgExtractor
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Insert folder with files : ");

            string path;
            path = Console.ReadLine();
            var dir = new Paths(path);
            var imgs = new ImageSearcher(dir.FilePathsList);
            await imgs.copyImagesAsync();

            Console.WriteLine("Done!");
        }
    }
}
