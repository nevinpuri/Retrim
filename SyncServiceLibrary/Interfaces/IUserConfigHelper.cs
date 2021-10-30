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
        UserConfig GetUserConfig();

        string GetVideoPath();

        Task SetUserConfig(UserConfig config);

        string GetThumbnailPath();
    }
}
