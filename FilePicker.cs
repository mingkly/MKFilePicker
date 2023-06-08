using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKFilePicker
{
    public partial class FilePicker : IFilePicker
    {
        public  FilePickResult? CreateFile(string platformFolderPath, string childPath)
        {
            return CreateFilePlatform(platformFolderPath, childPath);
        }

        public FilePickResult? CreateFolder(string platformFolderPath, string childPath)
        {
            return CreateFolderPlatform(platformFolderPath, childPath);
        }



        public Stream? OpenPickedFile(string platformPath, string fileOpenMode)
        {
            return OpenPickedFilePlatform(platformPath,fileOpenMode);
        }

        public Task<FilePickResult?> PickFileAsync(FilePickOptions? pickOptions)
        {
            return PickFilePlatformAsync(pickOptions);
        }

        public Task<IEnumerable<FilePickResult>> PickFilesAsync(FilePickOptions? pickOptions)
        {
            return PickFilesPlatformAsync(pickOptions);
        }

        public Task<FilePickResult?> PickFolderAsync(FilePickOptions? pickOptions)
        {
            return PickFolderPlatformAsync(pickOptions);
        }
    }
}
