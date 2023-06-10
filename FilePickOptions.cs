using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
#nullable enable
namespace MKFilePicker
{
    public class FilePickOptions:PickOptions
    {
        static FilePickerFileType[] FilePickerFileTypes = new FilePickerFileType[]
{
        new FilePickerFileType(new Dictionary<DevicePlatform,IEnumerable<string>>
        {
            {DevicePlatform.Android,new string[]{"image/*"} },
            {DevicePlatform.WinUI,new string[]{"*.png", "*.jpg", "*.jpeg", "*.webp","*.gif","*.bmp","" } }
        }),
        new FilePickerFileType(new Dictionary<DevicePlatform,IEnumerable<string>>
        {
            {DevicePlatform.Android,new string[]{"audio/*"} },
            {DevicePlatform.WinUI,new string[]{ "*.mp3", "*.wav", "*.flac", "*.m4a", "*.midi", "*.ogg", "*.ape", "*.alac", "*.aac", } }
        }),
        new FilePickerFileType(new Dictionary<DevicePlatform,IEnumerable<string>>
        {
            {DevicePlatform.Android,new string[]{"video/*"} },
            {DevicePlatform.WinUI,new string[]{ "*.mp4", "*.rmvb", "*.mkv", "*.3gp", "*.wmv", "*.mov", "*.rm", "*.flv" } }
        }),
        new FilePickerFileType(new Dictionary<DevicePlatform,IEnumerable<string>>
        {
            {DevicePlatform.Android,new string[]{"*/*"} },
            {DevicePlatform.WinUI,new string[]{ "" } }
        }),
        new FilePickerFileType(new Dictionary<DevicePlatform,IEnumerable<string>>
        {
            {DevicePlatform.Android,new string[]{"text/*","application/*"} },
            {DevicePlatform.WinUI,new string[]{ "*.txt","*.csv", "*.lrc", "*.srt", "*.ass", } }
        }),
};
        public static new FilePickOptions Images = new FilePickOptions
        {
            FileTypes = FilePickerFileTypes[0]
        };
        public static FilePickOptions Audios = new FilePickOptions
        {
            FileTypes = FilePickerFileTypes[1]
        };
        public static FilePickOptions Videos = new FilePickOptions
        {
            FileTypes = FilePickerFileTypes[2]
        };
        public static FilePickOptions Texts = new FilePickOptions
        {
            FileTypes = FilePickerFileTypes[4]
        };
        /// <summary>
        /// 选择是否保留权限,应用重启后也可访问
        /// take persistable permission after application rstart or not
        /// </summary>
        public bool HoldPermission { get; set; } = true;
    }
}
