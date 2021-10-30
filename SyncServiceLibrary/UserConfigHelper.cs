using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ResyncService.Interfaces;
using ResyncService.Models;

namespace ResyncService
{
    public class UserConfigHelper : IUserConfigHelper
    {
        public UserConfig GetUserConfig()
        {
            string userConfigFile = File.ReadAllText("C:\\Users\\Nevin\\Desktop\\resync\\userConfig.json"); // here
            UserConfig userConfig = JsonConvert.DeserializeObject<UserConfig>(userConfigFile);
            return userConfig;
        }

        public async Task SetUserConfig(UserConfig config)
        {
            string serializedConfig = JsonConvert.SerializeObject(config);
            await File.WriteAllTextAsync($"C:\\Users\\Nevin\\Desktop\\resync\\userConfig.json", serializedConfig);
        }

        public string GetVideoPath()
        {
            return GetUserConfig().FolderLocation;
        }

        public string GetThumbnailPath()
        {
            return GetUserConfig().ThumbnailLocation;
        }

    }
}
