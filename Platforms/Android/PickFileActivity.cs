using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Provider;
using AndroidX.DocumentFile.Provider;
using Microsoft.Maui.Controls.PlatformConfiguration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MKFilePicker
{
    [Activity(MainLauncher = false, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
    internal class PickFileActivity : Activity
    {
        static TaskCompletionSource<FilePickResult?>? pickFileTaskCompletionSource;
        public static TaskCompletionSource<FilePickResult?>? PickFileTaskCompletionSource
        {
            get => pickFileTaskCompletionSource;
            set
            {
                pickFileTaskCompletionSource = value;
                ActionId = PickFileId;
            }
        }
        public readonly static int PickFileId = 1;
        static TaskCompletionSource<IEnumerable<FilePickResult>>? pickFilesTaskCompletionSource;
        public static TaskCompletionSource<IEnumerable<FilePickResult>>? PickFilesTaskCompletionSource
        {
            get { return pickFilesTaskCompletionSource; }
            set
            {
                pickFilesTaskCompletionSource = value;
                ActionId = PickFilesId;
            }
        }
        public readonly static int PickFilesId = 2;
        static TaskCompletionSource<FilePickResult?>? pickFolderTaskCompletionSource;
        public static TaskCompletionSource<FilePickResult?>? PickFolderTaskCompletionSource
        {
            get => pickFolderTaskCompletionSource;
            set
            {
                pickFolderTaskCompletionSource = value;
                ActionId = PickFolderId;
            }
        }
        public readonly static int PickFolderId = 3;

        public static string? MimeType { get; set; }
        public static string[]? ExtraMimeTypes { get; set; }
        static int ActionId { get; set; }
        public static bool HoldPermisson {get;set;}
        public static string? DisplayTitle { get; set; }
        protected override void OnCreate(Bundle? savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Intent intent;
            MimeType ??= "*/*";
            switch (ActionId)
            {
                case 1:
                    intent = new Intent(Intent.ActionOpenDocument);
                    DisplayTitle ??= "选择文件";
                    SetMimeType(intent);                
                    intent.PutExtra(Intent.ExtraTitle, DisplayTitle);
                    StartActivityForResult(intent, PickFileId);
                    break;
                case 2:
                    intent = new Intent(Intent.ActionOpenDocument);
                    SetMimeType(intent);
                    DisplayTitle ??= "选择多个文件";
                    intent.PutExtra(Intent.ExtraTitle, DisplayTitle);
                    intent.PutExtra(Intent.ExtraAllowMultiple, true);
                    StartActivityForResult(intent, PickFilesId);
                    break;
                case 3:
                    intent = new Intent(Intent.ActionOpenDocumentTree);
                    DisplayTitle ??= "选择文件夹";
                    intent.PutExtra(Intent.ExtraTitle, DisplayTitle);
                    StartActivityForResult(intent,PickFolderId);
                    break;
            }         
            this.SetContentView(new Android.Widget.LinearLayout(this));
        }
        static void SetMimeType(Intent intent)
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.Kitkat)
            {
                intent.SetType(MimeType);
                if (ExtraMimeTypes != null&&ExtraMimeTypes.Length>1)
                {
                    intent.SetType("*/*");
                    intent.PutExtra(Intent.ExtraMimeTypes,ExtraMimeTypes);
                }
            }
            else
            {
                intent.SetType(MimeType);
                if (ExtraMimeTypes != null && ExtraMimeTypes.Length > 1)
                {
                    intent.SetType(string.Join("|",ExtraMimeTypes));
                }
            }
        }
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent? data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            if (requestCode == PickFileId)
            {
                try
                {
                    if (resultCode == Result.Ok&&data!=null)
                    {
                        var uri = data.Data;
                        var takeFlags = ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission;
                        if (uri != null)
                        {
                            if (HoldPermisson)
                            {
                                ContentResolver?.TakePersistableUriPermission(uri, takeFlags);
                            }                           
                            PickFileTaskCompletionSource?.TrySetResult(ReadFile(uri));
                        }                      
                    }
                }
                catch { }
                PickFileTaskCompletionSource?.TrySetResult(null);
            }
            else if (requestCode == PickFilesId)
            {
                try
                {
                    if (resultCode == Result.Ok&&data?.ClipData!=null)
                    {
                        var results = new List<FilePickResult>();
                        for (int i = 0; i < data.ClipData.ItemCount; i++)
                        {
                            var uri = data.ClipData.GetItemAt(i);
                            var takeFlags = ActivityFlags.GrantReadUriPermission|ActivityFlags.GrantWriteUriPermission;
                            if (uri?.Uri != null)
                            {
                                if (HoldPermisson)
                                {
                                    ContentResolver?.TakePersistableUriPermission(uri.Uri, takeFlags);
                                }
                                try
                                {
                                    results.Add(ReadFile(uri.Uri));
                                }
                                catch { }
                            }                      

                        }
                        PickFilesTaskCompletionSource?.TrySetResult(results);
                    }
                }
                catch { }
                PickFilesTaskCompletionSource?.TrySetResult(new List<FilePickResult>());
            }
            else if (requestCode == PickFolderId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {

                    Android.Net.Uri? uri = data.Data;
                    var takeFlags = ActivityFlags.GrantReadUriPermission | ActivityFlags.GrantWriteUriPermission;
                    if (uri != null)
                    {
                        if (HoldPermisson)
                        {
                            ContentResolver?.TakePersistableUriPermission(uri, takeFlags);
                        }
                        var folderName=uri.Path?.Split(":").Last();
                        PickFolderTaskCompletionSource?.TrySetResult(
                            new FilePickResult(folderName,GetAbsoluteFolderPath(uri),uri.ToString()));
                    }
                }
                else
                {
                    PickFolderTaskCompletionSource?.TrySetResult(null);
                }
            }
            Finish();
        }
        FilePickResult ReadFile(Android.Net.Uri uri)
        {
            var documentFile = DocumentFile.FromSingleUri(this, uri);           
            return new FilePickResult(documentFile?.Name, GetAbsoluteFolderPath(uri), uri.ToString());
        }
        public static string? GetAbsolutePath(Android.Net.Uri uri)
        {
            using var cusor = Android.App.Application.Context.ContentResolver?.Query(uri,
                new string[] { "_data" }, null, null, null);
            if(cusor != null&&cusor.MoveToNext())
            {
                var dataCol=cusor.GetColumnIndex("_data");
                return cusor.GetString(dataCol);
            }
            return uri.Path;
        }
        public static string? GetAbsoluteFolderPath(Android.Net.Uri uri)
        {
            var path=uri?.Path?.Split(':').Last();
            if (path != null)
            {
                return System.IO.Path.Combine(Android.OS.Environment.ExternalStorageDirectory?.AbsolutePath??string.Empty, path);
            }
            return uri?.Path;
        }
    }
}
