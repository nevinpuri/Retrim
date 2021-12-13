using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SyncServiceLibrary.Interfaces
{
    public interface ISyncService
    {
        public Task<int> QueryAllVideos();

        public Task<List<VideoFile>> GetAllUserClips();

        public int GenerateAllThumbnails();

        public Task<List<string>> GetAllGames();
    }
}
