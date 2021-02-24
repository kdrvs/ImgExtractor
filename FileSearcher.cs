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
        private List<string> TypeFilesList = new List<string>();
        private FileType Type;
       
        public FileSearcher(List<string> pathList, FileType type )
        {
            Type = type;
            getTypePathsList(pathList);
            hash = (type.GetHashCode() + (new Random()).Next(1, 1000)).ToString();
        }

        private void getTypePathsList(List<string> paths)
        {
            foreach (string path in paths)
            {
                if (validator(path, Type.Signatures, Type.Offset))
                    this.TypeFilesList.Add(path);
            }
            if(this.TypeFilesList.Count > 0)
                Console.WriteLine("Found of " + Type.FileExtension + " Files = "+ this.TypeFilesList.Count);
        }


        public void copyImages()
        {
            try
            {
                if (this.TypeFilesList.Count > 0 && !Directory.Exists(Type.FileExtension))
                     Directory.CreateDirectory(Type.FileExtension);

                var index = 0;
                foreach (string path in this.TypeFilesList)
                {
                    File.Copy(path, Type.FileExtension + "/" + hash + index.ToString() + "." + Type.FileExtension.ToLower());
                    index++;

                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            
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
                    using (Stream source = File.Open(path, FileMode.Open))
                    {
                        using (Stream target = File.Create(Type.FileExtension + "/" + hash + index.ToString() + "." + Type.FileExtension.ToLower()))
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

        private bool validator(string path, string[] signatures, int offset)
        {
            if (!File.Exists(path))
                return false;

            byte[] buffer = new byte[signatures.Length];
            string[] fileHeader = new string[signatures.Length];
            using (FileStream stream = File.OpenRead(path))
            {
                stream.Seek(offset, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);
                for(int i = 0; i < fileHeader.Length; i++)
                {
                    fileHeader[i] = buffer[i].ToString("X2");
                }
                /*for (int i = Type.StartingByte; i < signatures.Length + Type.StartingByte; i++)
                {
                    Console.WriteLine(stream.ReadByte().ToString("X2")); //WTF??
                    fileHeader[i] = (stream.ReadByte().ToString("X2"));
                }*/
                
                

                if (Enumerable.SequenceEqual(fileHeader, signatures))
                    return true;
            }
                         

            return false;

        }


    }
}