using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SyncServiceLibrary.Models;

namespace SyncServiceLibrary.Interfaces
{
    public interface IUserConfigHelper
    {
        SettingConfig GetUserConfig();

        Task SetUserConfig(SettingConfig config);

        string GetVideoPath();

        string GetThumbnailPath();

        void CreateUserConfig();

        void ResetUserConfig();

        bool CheckUserConfig();

        string GetTempPath();

        string GetConfigLocation();

        Task SetInitialStart();
    }
}
