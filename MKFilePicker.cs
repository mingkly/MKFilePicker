using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKFilePicker
{
    public static class MKFilePicker
    {
        static FilePicker filePicker;
        public static IFilePicker Default
        {
            get
            {
                if(filePicker == null)
                {
                    filePicker = new FilePicker();
                }
                return filePicker;
            }
        }
        /// <summary>
        /// pick mutilple files
        /// </summary>
        /// <param name="pickOptions">can be null</param>
        /// <returns></returns>
        public static Task<IEnumerable<FilePickResult>> PickFilesAsync(FilePickOptions? pickOptions)=>Default.PickFilesAsync(pickOptions);
        /// <summary>
        /// pick single file
        /// </summary>
        /// <param name="pickOptions">can be null</param>
        /// <returns></returns>
        public static Task<FilePickResult?> PickFileAsync(FilePickOptions? pickOptions)=>Default.PickFileAsync(pickOptions);
        /// <summary>
        /// pick a folder to create file in it
        /// </summary>
        /// <param name="pickOptions">set display title</param>
        /// <returns></returns>
        public static Task<FilePickResult?> PickFolderAsync(FilePickOptions? pickOptions)=>Default.PickFolderAsync(pickOptions);
        /// <summary>
        /// open picked file
        /// </summary>
        /// <param name="platformPath">the path from picked result</param>
        /// <returns></returns>
        public static Stream? OpenPickedFile(string platformPath)=>Default.OpenPickedFile(platformPath);
        /// <summary>
        /// create file
        /// </summary>
        /// <param name="platformFolderPath">the path from picked result or created from CreateFolder</param>
        /// <param name="childPath">"folder/file.txt or file.txt. "/"is Path.PathSeparator,In Android is ":" "</param>
        /// <returns></returns>
        public static FilePickResult? CreateFile(string platformFolderPath, string childPath)=>Default.CreateFile(platformFolderPath, childPath);
        /// <summary>
        /// create folder
        /// </summary>
        /// <param name="platformFolderPath">the path from picked result or created from CreateFolder</param>
        /// <param name="childPath">"folder/innerFodler or folder. "/"is Path.PathSeparator,In Android is ":" "</param>
        /// <returns></returns>
        public static FilePickResult? CreateFolder(string platformFolderPath, string childPath)=>Default.CreateFolder(platformFolderPath, childPath);
    }
}
