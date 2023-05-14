using Android.App;
using Android.Content;
using Android.OS;
using Android.Provider;
using AndroidX.DocumentFile.Provider;
using Java.Nio.FileNio.Attributes;
using Microsoft.Maui.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;


namespace MKFilePicker
{
    public partial class FilePicker
    {
        Context Context => Android.App.Application.Context;
        internal Stream? OpenPickedFilePlatform(string platformPath)
        {
            if (platformPath.StartsWith("content"))
            {
                var descriptor = Context.ContentResolver?.OpenFileDescriptor(Android.Net.Uri.Parse(platformPath)!, "rw");
                return new JavaStreamWrapper(descriptor!);
            }
            else
            {
                try
                {
                    return System.IO.File.Open(platformPath, FileMode.OpenOrCreate);
                }
                catch { }
                try
                {
                    var status = Permissions.CheckStatusAsync<Permissions.StorageRead>().Result;
                    if (status != PermissionStatus.Granted)
                    {
                        Permissions.RequestAsync<Permissions.StorageRead>().Wait();
                        Permissions.RequestAsync<Permissions.StorageWrite>().Wait();
                    }
                    var file = new Java.IO.File(platformPath);
                    if (file.Exists())
                    {
                        return new JavaStreamWrapper(file);
                    }
                }
                catch { }
                return null;
            }
        }

        internal Task<FilePickResult?> PickFilePlatformAsync(FilePickOptions? pickOptions)
        {
            PickFileActivity.PickFileTaskCompletionSource = new TaskCompletionSource<FilePickResult?>();
            PickFileActivity.HoldPermisson = pickOptions?.HoldPermission ?? true;
            PickFileActivity.DisplayTitle = pickOptions?.PickerTitle;
            if (pickOptions != null)
            {
                PickFileActivity.MimeTypes = string.Join(";", pickOptions?.FileTypes?.Value ?? new string[] { "*/*" });
            }
            else
            {
                PickFileActivity.MimeTypes = "*/*";
            }
            var intent = new Intent(Context, typeof(PickFileActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Context.StartActivity(intent);
            return PickFileActivity.PickFileTaskCompletionSource.Task;
        }

        internal Task<IEnumerable<FilePickResult>> PickFilesPlatformAsync(FilePickOptions? pickOptions)
        {
            PickFileActivity.PickFilesTaskCompletionSource = new TaskCompletionSource<IEnumerable<FilePickResult>>();
            PickFileActivity.HoldPermisson = pickOptions?.HoldPermission ?? true;
            PickFileActivity.DisplayTitle = pickOptions?.PickerTitle;
            if (pickOptions != null)
            {
                PickFileActivity.MimeTypes = string.Join(";", pickOptions?.FileTypes?.Value ?? new string[] { "*/*" });
            }
            else
            {
                PickFileActivity.MimeTypes = "*/*";
            }
            Context.StartActivity(typeof(PickFileActivity));
            return PickFileActivity.PickFilesTaskCompletionSource.Task;
        }

        internal Task<FilePickResult?> PickFolderPlatformAsync(FilePickOptions? pickOptions)
        {
            PickFileActivity.PickFolderTaskCompletionSource = new TaskCompletionSource<FilePickResult?>();
            PickFileActivity.HoldPermisson = pickOptions?.HoldPermission ?? true;
            PickFileActivity.DisplayTitle = pickOptions?.PickerTitle ?? "选择文件夹";
            var intent = new Intent(Context, typeof(PickFileActivity));
            intent.SetFlags(ActivityFlags.NewTask);
            Context.StartActivity(intent);
            return PickFileActivity.PickFolderTaskCompletionSource.Task;
        }

        internal FilePickResult? CreateFilePlatform(string platformFolderPath, string childPath)
        {
            var paths = childPath.Split(System.IO.Path.PathSeparator);
            var folderUri = Android.Net.Uri.Parse(platformFolderPath);
            if (folderUri != null && paths.Length > 0)
            {
                DocumentFile? file;
                file = DocumentFile.FromTreeUri(Context, folderUri);
                if (file != null)
                {
                    if (paths.Length > 1)
                    {
                        file = CreateStructureFoldersInDocument(paths.Take(paths.Length - 1), file);
                        return CreateFileInDocument(paths[paths.Length - 1], file);
                    }
                    else if (paths.Length == 1)
                    {
                        return CreateFileInDocument(paths[0], file);
                    }
                }
            }
            return null;
        }

        internal FilePickResult? CreateFolderPlatform(string platformFolderPath, string childPath)
        {
            var paths = childPath.Split(System.IO.Path.PathSeparator);
            var folderUri = Android.Net.Uri.Parse(platformFolderPath);
            if (folderUri != null && paths.Length > 0)
            {
                DocumentFile? file;
                file = DocumentFile.FromTreeUri(Context, folderUri);
                if (file != null)
                {
                    if (paths.Length > 1)
                    {
                        file = CreateStructureFoldersInDocument(paths.Take(paths.Length - 1), file);
                        return CreateFolderInDocument(paths[paths.Length - 1], file);
                    }
                    else if (paths.Length == 1)
                    {
                        return CreateFolderInDocument(paths[0], file);
                    }
                }
            }
            return null;
        }

        FilePickResult? CreateFileInDocument(string fileName, DocumentFile? file)
        {
            var target = file?.FindFile(fileName);
            if (target == null || target.IsDirectory)
            {
                file = file?.CreateFile("*/*", fileName)!;
            }
            else
            {
                file = target;
            }
            if (file != null)
            {
                //Context?.ContentResolver?.TakePersistableUriPermission(file.Uri, ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission );
                return new FilePickResult(file.Name,
                    file.Uri.Path, file.Uri.ToString());
            }

            return null;
        }

        FilePickResult? CreateFolderInDocument(string folderName, DocumentFile? file)
        {

            var target = file?.FindFile(folderName);
            if (target == null || target.IsFile)
            {
                file = file?.CreateDirectory(folderName);
            }
            else
            {
                file = target;
            }
            if (file != null)
            {
                //Context?.ContentResolver?.TakePersistableUriPermission(file.Uri, ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                return new FilePickResult(file.Name,
                    file.Uri.Path, file.Uri.ToString());
            }
            return null;
        }

        DocumentFile? CreateStructureFoldersInDocument(IEnumerable<string> folders, DocumentFile? file)
        {
            foreach (var folder in folders)
            {
                var target = file?.FindFile(folder);
                if (target == null || target.IsFile)
                {
                    file = file?.CreateDirectory(folder);
                }
                else
                {
                    file = target;
                }
                if (file != null)
                {
                    //Context?.ContentResolver?.TakePersistableUriPermission(file.Uri, ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission);
                }
            }
            return file;
        }
    }
}
