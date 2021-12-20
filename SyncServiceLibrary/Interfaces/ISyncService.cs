using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyncServiceLibrary.Interfaces
{
    public interface ISyncService
    {
        public Task<int> QueryAllVideos();

        public Task ReIndexClips();

        public Task ResetClips();

        public Task<List<VideoFile>> GetAllUserClips(string filter);

        public Task<int> GenerateAllThumbnails();

        public Task<List<string>> GetAllGames();
    }
}
