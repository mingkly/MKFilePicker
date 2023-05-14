using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKFilePicker
{
    public class FilePickResult
    {
        public string? FileName { get; }
        public string? FullPath { get; }
        /// <summary>
        /// platform path for read or write
        /// 平台路径，供读写随用
        /// </summary>
        public string? PlatformPath { get; }
        public FilePickResult(string? fileName, string? fullPath, string? platformPath)
        {
            FileName = fileName;
            FullPath = fullPath;
            PlatformPath = platformPath;
        }
    }
}
