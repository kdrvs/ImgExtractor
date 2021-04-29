using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Linq;

namespace ImgExtractor
{
    class FileMapper
    {
        private List<string> pathes;
        public ConcurrentDictionary<string, FileType> filesMap;
        private List<FileType> fileTypes;

        private string targetPath;
        private int countOFCopedFiles = 0;
        private DateTime startTime = DateTime.Now;
        private DateTime mapperWorked = DateTime.Now;
        private DateTime totalTime = DateTime.Now;

        public FileMapper(List<string> pathes, List<FileType> fileTypes, string _targetPath)
        {
            this.filesMap = new ConcurrentDictionary<string, FileType>();
            this.pathes = pathes;
            this.fileTypes = fileTypes;
            this.targetPath = _targetPath;
            createTargetFolder(_targetPath);
        }

        private void createTargetFolder(string path)
        {
            if(!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
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
            await Task.WhenAll(pathes.Select(p => checkFile(p)));
            
            this.mapperWorked = DateTime.Now;
            this.totalTime = this.mapperWorked;
           
        }

        private async Task checkFile(string path)
        {
            var fileType = await getFileAsync(path);
            
                if (fileType.FileExtension != "None")
                {    
                    this.filesMap.TryAdd(path, fileType);
                    
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
            await Task.WhenAll(this.filesMap.Select(f => copyFileAsync(f)));

            this.totalTime = DateTime.Now;
       
        }

        private async Task copyFileAsync(KeyValuePair<string, FileType> file)
        {
            var sourcePath = file.Key;
            var targetFileName = Math.Abs(file.Value.GetHashCode()).ToString()
                + Math.Abs(startTime.GetHashCode()).ToString()
                + this.countOFCopedFiles.ToString();
            var targetDirectory = this.targetPath + "/" + file.Value.FileExtension;
            string _targetPath;

            this.countOFCopedFiles ++;

            try
            {
                if (!Directory.Exists(targetDirectory))
                    Directory.CreateDirectory(targetDirectory);

                _targetPath = targetDirectory + '/' + targetFileName + '.' + file.Value.FileExtension.ToLower();
                using (FileStream source = new FileStream(sourcePath, FileMode.Open))
                {
                    using(FileStream target = new FileStream(_targetPath, FileMode.OpenOrCreate))
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

        public void saveLog()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter("Log.txt", true))
                {
                    writer.WriteLine("\n" + "***");
                    writer.WriteLine(DateTime.Now.ToString("G"));
                    var map = getAmountMap();
                    foreach (KeyValuePair<string, int> m in map)
                    {
                        writer.WriteLine("Found of {0} : {1} ", m.Value, m.Key);
                    }

                    writer.WriteLine("Mapper worked : {0} sec", (this.mapperWorked - this.startTime).TotalSeconds.ToString());

                    writer.WriteLine("Total time : {0} sec", (this.totalTime - this.startTime).TotalSeconds.ToString());
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
       
    }
}
