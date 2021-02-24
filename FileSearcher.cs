using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace ImgExtractor
{
    public class FileSearcher
    {

        private string hash;
        private List<string> paths;
        private List<string> TypeFilesList = new List<string>();
        private FileType Type;
       
        public FileSearcher(List<string> pathList, FileType type )
        {
            paths = pathList;
            Type = type;
            hash = (type.GetHashCode() + (new Random()).Next(1, 1000)).ToString();
        }

        public static async Task<FileSearcher> CreateAsync(List<string> pathList, FileType type)
        {
            var fileSearcher = new FileSearcher(pathList, type);
            await fileSearcher.extractFileTypePathsListAsync();
            return fileSearcher;
        }

        public async Task extractFileTypePathsListAsync()
        {
            foreach (string path in paths)
            {
                if (await validatorAsync(path, Type.Signatures, Type.Offset))
                    this.TypeFilesList.Add(path);
            }
            if (this.TypeFilesList.Count > 0)
                Console.WriteLine("Found of " + Type.FileExtension + " Files: " + this.TypeFilesList.Count);
        }

        
        public async Task copyImagesAsync()
        {
            try
            {
                if (this.TypeFilesList.Count > 0 && !Directory.Exists(Type.FileExtension))
                    Directory.CreateDirectory(Type.FileExtension);

                var index = 0;
                foreach (string path in this.TypeFilesList)
                {
                    using(FileStream source = new FileStream(path, FileMode.Open))
                    {
                        using(FileStream target = new FileStream((Type.FileExtension + "/" + hash + index.ToString() + "." + Type.FileExtension.ToLower()), FileMode.OpenOrCreate))
                        {
                            await source.CopyToAsync(target);
                            index++;
                        }
                    }

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        private async Task<bool> validatorAsync(string path, string[] signatures, int offset)
        {
            if (!File.Exists(path))
                return false;

            byte[] buffer = new byte[signatures.Length];
            string[] fileHeader = new string[signatures.Length];
            using (FileStream stream = File.OpenRead(path))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                await stream.ReadAsync(buffer, 0, buffer.Length);
                for (int i = 0; i < fileHeader.Length; i++)
                {
                    fileHeader[i] = buffer[i].ToString("X2");
                }

                if (Enumerable.SequenceEqual(fileHeader, signatures))
                    return true;
            }

            return false;

        }

        public void moveImages()
        {
            try
            {
                if (this.TypeFilesList.Count > 0 && !Directory.Exists(Type.FileExtension))
                    Directory.CreateDirectory(Type.FileExtension);

                var index = 0;
                foreach (string path in this.TypeFilesList)
                {
                    File.Move(path, Type.FileExtension + "/" + hash + index.ToString() + "." + Type.FileExtension.ToLower());
                    index++;

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }
    }
}