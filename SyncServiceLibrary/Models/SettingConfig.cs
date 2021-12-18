using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncServiceLibrary.Models
{
    public class SettingConfig
    {
        public bool InitialStart { get; set; }
        public bool CheckForUpdates { get; set; }

        public string UpdateServer { get; set; }

        public bool CompressVideos { get; set; }

        public string FolderLocation { get; set; }

        public string ThumbnailLocation { get; set; }
    }
}
