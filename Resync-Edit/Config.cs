using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Resync_Edit.Models;

namespace Resync_Edit
{
    public static class Config
    {
        public static bool CheckForUpdates { get; set; }

        public static string UpdateServer { get; set; }

        public static bool CompressVideos { get; set; }
    }
}
