# MKFilePicker
Modify Maui File picker. can pick folder,read pick file after restart application.create file under picked folder
Only support windows and Android.
基于Maui的文件选择器，可以选择文件夹，并且在程序重启后访问选择的文件或文件夹。

1.pick a video file and read it:
挑选文件并读取：
            FilePickResult res = await Picker.PickFileAsync(FilePickOptions.Videos);
            using var stream = Picker.OpenPickedFile(res.PlatformPath, "r");
if you target android api29 or lower and granted android.permission.READ_EXTERNAL_STORAGE and android.permission.WRITE_EXTERNAL_STORAGE,
or target android api29 higher and granted android.permission.MANAGE_EXTERNAL_STORAGE,
or target windows,you can read file like this:
如果目标安卓平台在api29以下并且获得了读写外部存储权限，或者api29以上并获得管理所有文件权限，或者windows平台，可以直接使用file api访问：
            using var fs=File.OpenRead(res.FullPath);
2，pick multiFiles:
选择多个文件：
            var results = await Picker.PickFilesAsync(FilePickOptions.Videos);
3，pick special type file:
选择特定文件：
            var fileOptions = new FilePickOptions()
            {
                FileTypes = new FilePickerFileType(new Dictionary<DevicePlatform, IEnumerable<string>>
                   {
                      {DevicePlatform.Android,new string[]{"image/*"} },
                      {DevicePlatform.WinUI,new string[]{"*.png", "*.jpg", "*.jpeg", "*.webp","*.gif","*.bmp"} }
                   }),
            };
            FilePickResult res = await Picker.PickFileAsync(fileOptions);
  4,pick a folder and create file under it:
  挑选文件夹，并在其下创建文件：
            var folder = await Picker.PickFolderAsync(null);
            var res = Picker.CreateFile(folder.PlatformPath, "test.txt");
            using var stream=Picker.OpenPickedFile(res.PlatformPath, "w");
            using var sw=new StreamWriter(stream );
            sw.Write("测试文字");
  5，pick a folder and create folder under it:
  挑选文件夹并在其下创建文件夹：
            var res3 = Picker.CreateFolder(folder.PlatformPath, "testFolder");
            var res4 = Picker.CreateFile(res3.PlatformPath, "TestInnerFolder/test.txt");
            using var stream = Picker.OpenPickedFile(res4.PlatformPath, "w");
            using var sw = new StreamWriter(stream);
            sw.Write("测试文字");
  
