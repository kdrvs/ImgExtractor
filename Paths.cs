using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ImgExtractor
{
    public class Paths
    {
        
        public List<string> FilePathsList 
        {
            get;
            private set;
        }

        public Paths(string path)
        {
            extract(path);
        }

        private void extract(string path)
        {
            try
            {
                FilePathsList = Directory.GetFiles(path, "*", SearchOption.AllDirectories).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                
            }
            
        }
    }
}