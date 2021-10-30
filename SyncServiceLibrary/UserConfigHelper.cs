﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using SyncServiceLibrary.Interfaces;
using SyncServiceLibrary.Models;

namespace SyncServiceLibrary
{
    public class UserConfigHelper : IUserConfigHelper
    {

        readonly string resyncPath = Path.Join(Path.GetTempPath(), "resync");

        string currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public UserConfig GetUserConfig()
        {
            if (!CheckUserConfig()) CreateUserConfig();
            string userConfigFile = File.ReadAllText(GetConfigLocation()); // here
            UserConfig userConfig = JsonConvert.DeserializeObject<UserConfig>(userConfigFile);
            return userConfig;
        }

        public async Task SetUserConfig(UserConfig config)
        {
            string serializedConfig = JsonConvert.SerializeObject(config);
            await File.WriteAllTextAsync(GetConfigLocation(), serializedConfig);
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
            Directory.CreateDirectory(resyncPath);
            File.Create(Path.Join(resyncPath, "userConfig.json")).Dispose();
            Directory.CreateDirectory(Path.Join(resyncPath, "thumbnails"));
            SettingConfig settingConfig = new SettingConfig()
            {
                CheckForUpdates = true, CompressVideos = false,
                FolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                UpdateServer = "https://nevin.cc/resync/update",
                ThumbnailLocation = Path.Join(resyncPath, "thumbnails")
            };
            //File.WriteAllText(Path.Join(resyncPath, "userConfig.json"), "{\"CheckForUpdates\": true, \"UpdateServer\": \"https://nevin.cc/resync/update\", \"CompressVideos\": false, \"FolderLocation\": \"" + Environment.GetFolderPath(Environment.SpecialFolder.MyVideos) + "\"}");
            File.WriteAllText(Path.Join(resyncPath, "userConfig.json"), JsonConvert.SerializeObject(settingConfig));
            File.Copy(Path.Join(currentDir, "resyncDbContext.sqlite"), Path.Join(resyncPath, "resyncDbContext.sqlite"));
        }

        public void ResetUserConfig()
        {
            throw new NotImplementedException();
        }

        public bool CheckUserConfig()
        {
            if (!Directory.Exists(resyncPath))
                return false;
            if (!File.Exists(Path.Join(resyncPath, "userConfig.json")))
                return false;
            if (!Directory.Exists(Path.Join(resyncPath, "thumbnails")))
                return false;
            if (!File.Exists(Path.Join(resyncPath, "resyncDbContext.sqlite")))
                return false;
            return true;
        }

        public string GetTempPath()
        {
            return resyncPath;
        }

        public string GetConfigLocation()
        {
            return Path.Join(resyncPath, "userConfig.json");
        }

        public string GetDbPath()
        {
            return Path.Join(resyncPath, "resyncDbContext.sqlite");
        }

    }
}
