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

        /// <summary>
        /// 选择是否保留权限,应用重启后也可访问
        /// take persistable permission after application rstart or not
        /// </summary>
        public bool HoldPermission { get; set; } = true;
    }
}
