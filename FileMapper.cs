using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace ImgExtractor
{
    class FileMapper
    {
        private List<string> pathes;
        public Dictionary<string, FileType> filesMap = new Dictionary<string, FileType>();
        private List<FileType> fileTypes;

        public FileMapper(List<string> pathes, List<FileType> fileTypes)
        {
            this.pathes = pathes;
            this.fileTypes = fileTypes;
        }

        public Dictionary<string, int> getAmountMap()
        {
            var amount = new Dictionary<string, int>();
            foreach(FileType type in this.fileTypes)
            {
                int count = this.filesMap.Where(v => v.Value.FileExtension == type.FileExtension).Count();
                if (count > 0)
                {
                    if (!amount.ContainsKey(type.FileExtension))
                    {
                        amount.Add(type.FileExtension, count);
                    }

                }
                    
            }
            return amount;
        }

        public async Task searchFilesAsync()
        {
            var tasks = new List<Task>();
           
            foreach (string path in pathes)
            {
                tasks.Add(checkFile(path));
            }
            
            await Task.WhenAll(tasks);
           
        }

        private async Task checkFile(string path)
        {
            var fileType = await getFileAsync(path);
            if(fileType.FileExtension != "None")
            {
                this.filesMap.Add(path, fileType);
            }
        }

        private async Task<FileType> getFileAsync(string path)
        {
            var type = new FileType() { FileExtension = "None" };
            try
            {
                if (!File.Exists(path))
                    return type;

                byte[] buffer;
                string[] fileHeader;
                foreach (FileType fileType in this.fileTypes)
                {
                    buffer = new byte[fileType.Signatures.Length];
                    fileHeader = new string[fileType.Signatures.Length];

                    using (FileStream stream = File.OpenRead(path))
                    {
                        stream.Seek(fileType.Offset, SeekOrigin.Begin);
                        await stream.ReadAsync(buffer, 0, buffer.Length);
                        for (int i = 0; i < fileHeader.Length; i++)
                        {
                            fileHeader[i] = buffer[i].ToString("X2");
                        }
                        if (Enumerable.SequenceEqual(fileHeader, fileType.Signatures))
                            return fileType;
                    }
                }
                
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return type;

        }

        public async Task copyAsync()
        {
            var tasks = new List<Task>();
            
            foreach(KeyValuePair<string, FileType> file in this.filesMap)
            {
                tasks.Add(copyFileAsync(file));
            }
           
            await Task.WhenAll(tasks);
       
        }

        private async Task copyFileAsync(KeyValuePair<string, FileType> file)
        {
            var sourcePath = file.Key;
            var targetFileName = Math.Abs(file.GetHashCode());
            var targetDirectory = file.Value.FileExtension;
            string targetPath;

            try
            {
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);
                targetPath = targetDirectory + '/' + targetFileName + '.' + file.Value.FileExtension.ToLower();
                using (FileStream source = new FileStream(sourcePath, FileMode.Open))
                {
                    using(FileStream target = new FileStream(targetPath, FileMode.OpenOrCreate))
                    {
                        await source.CopyToAsync(target);
                    }
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

       
       
    }
}
