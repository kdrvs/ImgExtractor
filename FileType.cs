using System;
using System.Collections.Generic;
using System.Text;

namespace ImgExtractor
{
    public class FileType
    {
        public int Offset { get; set; } = 0;
        public string FileExtension { get; set; }
        public string[] Signatures { get; set; }
    }
}
