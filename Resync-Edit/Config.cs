using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Resync_Edit.Models;

namespace Resync_Edit
{
    public static class Config
    {
        public static bool CheckForUpdates { get; set; }

        public static string UpdateServer { get; set; }

        public static bool CompressVideos { get; set; }

        public static SettingsConfig GetConfig()
        {

            string configFile = File.ReadAllText(Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "userConfig.json"));
            return JsonConvert.DeserializeObject<SettingsConfig>(configFile);
        }
    }
}
