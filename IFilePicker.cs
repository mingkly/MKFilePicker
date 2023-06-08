using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace MKFilePicker
{
    public interface IFilePicker
    {
        /// <summary>
        /// pick mutilple files
        /// </summary>
        /// <param name="pickOptions">can be null</param>
        /// <returns></returns>
        Task<IEnumerable<FilePickResult>> PickFilesAsync(FilePickOptions? pickOptions);
        /// <summary>
        /// pick single file
        /// </summary>
        /// <param name="pickOptions">can be null</param>
        /// <returns></returns>
        Task<FilePickResult?> PickFileAsync(FilePickOptions? pickOptions);
        /// <summary>
        /// pick a folder to create file in it
        /// </summary>
        /// <param name="pickOptions">set display title</param>
        /// <returns></returns>
        Task<FilePickResult?> PickFolderAsync(FilePickOptions? pickOptions);
        /// <summary>
        /// open picked file
        /// </summary>
        /// <param name="platformPath">the path from picked result</param>
        /// <param name="fileOpenMode">"r","w","rw"</param>
        /// <returns></returns>
        Stream? OpenPickedFile(string platformPath,string fileOpenMode);

        /// <summary>
        /// create file
        /// </summary>
        /// <param name="platformFolderPath">the path from picked result or created from CreateFolder</param>
        /// <param name="childPath">"etc,folder/file.txt or file.txt"</param>
        /// <returns></returns>
        FilePickResult? CreateFile(string platformFolderPath, string childPath);
        /// <summary>
        /// create folder
        /// </summary>
        /// <param name="platformFolderPath">the path from picked result or created from CreateFolder</param>
        /// <param name="childPath">"etc,folder/file.txt or file.txt"</param>
        /// <returns></returns>
        FilePickResult? CreateFolder(string platformFolderPath, string childPath);
    }
}
