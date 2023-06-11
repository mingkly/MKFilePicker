using Microsoft.Maui.Storage;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Pickers;

namespace MKFilePicker
{
#nullable enable
    public partial class FilePicker
    {
        internal Stream? OpenPickedFilePlatform(string platformPath, string fileOpenMode)
        {
            if (fileOpenMode == "r")
            {
                return System.IO.File.OpenRead(platformPath);
            }
            else if (fileOpenMode == "w")
            {
                return System.IO.File.OpenWrite(platformPath);
            }
            else
            {
                return System.IO.File.Open(platformPath, FileMode.OpenOrCreate);
            }
        }

        internal async Task<FilePickResult?> PickFilePlatformAsync(FilePickOptions? pickOptions)
        {
            var res=await Microsoft.Maui.Storage.FilePicker.PickAsync(pickOptions);
            return res is null?null:new FilePickResult(res.FileName, res.FullPath, res.FullPath);
        }

        internal async Task<IEnumerable<FilePickResult>> PickFilesPlatformAsync(FilePickOptions? pickOptions)
        {
            var results = await Microsoft.Maui.Storage.FilePicker.PickMultipleAsync(pickOptions);
            return results is null?Array.Empty<FilePickResult>():results.Select(res => new FilePickResult(res.FileName, res.FullPath, res.FullPath));
        }

        internal async Task<FilePickResult?> PickFolderPlatformAsync(FilePickOptions? pickOptions)
        {
            var folderPicker = new FolderPicker();
            folderPicker.FileTypeFilter.Add("*");
            var hwnd = (Application.Current?.Windows[0].Handler.PlatformView as MauiWinUIWindow)?.WindowHandle;
            folderPicker.CommitButtonText=pickOptions?.PickerTitle?? folderPicker.CommitButtonText;
            // Associate the HWND with the file picker
            if(hwnd != null)
            {
                WinRT.Interop.InitializeWithWindow.Initialize(folderPicker, hwnd.Value);
                var folder = await folderPicker.PickSingleFolderAsync();
                if (folder?.Path==null)
                {
                    return null;
                }
                return new FilePickResult(folder.Name, folder.Path, folder.Path);
            }
            return null;
        }
        internal FilePickResult? CreateFilePlatform(string platformFolderPath, string childPath)
        {
            try
            {
                var path = Path.Combine(platformFolderPath, childPath);
                var fileName= Path.GetFileName(path);
                var folder = path.Replace(fileName, "");
                if(!Directory.Exists(folder))
                {
                    Directory.CreateDirectory(folder);
                }
                if (!File.Exists(path))
                {
                    using (File.Create(path)) { }
                }
                return new FilePickResult(Path.GetFileName(path), path, path);
            }
            catch(Exception ex)
            {
                Debug.WriteLine(ex);
            }
            return null;
        }

        internal FilePickResult? CreateFolderPlatform(string platformFolderPath, string childPath)
        {
            try
            {
                var path = Path.Combine(platformFolderPath, childPath);
                if (!Directory.Exists(path))
                {
                    _= Directory.CreateDirectory(path);
                }
                return new FilePickResult(Path.GetFileName(path.TrimEnd('/')) ?? childPath, path, path);
            }
            catch { }
            return null;
        }
    }
}
