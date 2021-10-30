using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncServiceLibrary.Interfaces;
using SyncServiceLibrary.Models;

namespace SyncServiceLibrary
{
    public class UserConfigHelper : IUserConfigHelper
    {
        public UserConfig GetUserConfig()
        {
            if (!CheckUserConfig()) CreateUserConfig();
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

        public void CreateUserConfig()
        {
            string resyncPath = Path.Join(Path.GetTempPath(), "resync");
            Directory.CreateDirectory(resyncPath);
            throw new NotImplementedException();
        }

        public void ResetUserConfig()
        {
            throw new NotImplementedException();
        }

        public bool CheckUserConfig()
        {
            string resyncPath = Path.Join(Path.GetTempPath(), "resync");
            if (!Directory.Exists(resyncPath))
                return false;
            if (!File.Exists(Path.Join(resyncPath, "userConfig.json")))
                return false;
            if (!Directory.Exists(Path.Join(resyncPath, "thumbnails")))
                return false;
            return true;
        }

    }
}
