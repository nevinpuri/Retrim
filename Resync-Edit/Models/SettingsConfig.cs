using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resync_Edit.Models
{
    public class SettingsConfig
    {
        public bool CheckForUpdates { get; set; }

        public string UpdateServer { get; set; }

        public bool CompressVideos { get; set; }
    }
}
