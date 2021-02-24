using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace ImgExtractor
{
    public class Signatures
    {
        private static string dataFile = "signatures.json";

        public static async Task<List<FileType>> getTypeListAsync()
        {
            List<FileType> json = new List<FileType>();
            try
            {
                using(FileStream stream = new FileStream(dataFile, FileMode.Open))
                {
                    json = await JsonSerializer.DeserializeAsync<List<FileType>>(stream);
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;
        }
        public static async Task<List<FileType>> getTypeList()
        {
            string json = await readJson();
            return JsonSerializer.Deserialize<List<FileType>>(json);
        }

        private static async Task<string> readJson()
        {
            string json = "";
            try
            {
                using (FileStream stream = new FileStream(dataFile, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        json = await reader.ReadToEndAsync();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            return json;
        }
    }
}
