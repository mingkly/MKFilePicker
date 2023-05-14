using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKFilePicker
{
    public partial class FilePicker
    {
        internal Stream? OpenPickedFilePlatform(string platformPath)
        {
            throw new NotImplementedException();
        }

        internal Task<FilePickResult?> PickFilePlatformAsync(FilePickOptions? pickOptions)
        {
            throw new NotImplementedException();
        }

        internal Task<IEnumerable<FilePickResult>> PickFilesPlatformAsync(FilePickOptions? pickOptions)
        {
            throw new NotImplementedException();
        }

        internal Task<FilePickResult?> PickFolderPlatformAsync(FilePickOptions? pickOptions)
        {
            throw new NotImplementedException();
        }
        internal FilePickResult? CreateFilePlatform(string platformFolderPath, string childPath)
        {
            throw new NotSupportedException();
        }

        internal FilePickResult? CreateFolderPlatform(string platformFolderPath, string childPath)
        {
            throw new NotSupportedException();
        }
    }
}
