using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAPICodePack.Shell;
using SyncServiceLibrary.Models;
using SyncServiceLibrary.Interfaces;

namespace SyncServiceLibrary
{

    public class SyncService : ISyncService
    {

        private readonly IUserConfigHelper _configHelper;

        private readonly ClipContext _clipContext;

        public SyncService(ClipContext clipContext, IUserConfigHelper configHelper) // removed the logger because it broke some stuff, I probably just need to create a new logger service on the main window so that I can use it again
        {
            _clipContext = clipContext;
            _configHelper = configHelper;
        }

        public async Task<int> QueryAllVideos()
        {
            int allGameAmount = 0;
            IEnumerable<string> allGames = Directory.GetDirectories(_configHelper.GetVideoPath())
                .Select(d => new DirectoryInfo(d).Name);
            foreach (string game in allGames)
            {
                foreach (string file in Directory.GetFiles(Path.Combine(_configHelper.GetVideoPath(), game), "*.mp4") // use Path.Combine
                ) // you can also add a filter to have all of the video formats be detected only
                {
                    string
                        fileName = Path.GetFileName(file); // make sure you also save the file size in the model as well
                    if (!_clipContext.VideoFiles.Any(b => b.Title == fileName))
                    {
                        allGameAmount++;
                        _clipContext.VideoFiles.Add(new VideoFile(fileName, game, // make sure to have a check for the timespan from seconds if the bool returned false, so that you can still generate a thumbnail for videos which are under five seconds
                            (new FileInfo(file).Length) / 1024 / 1024, file,
                        Path.Combine(_configHelper.GetThumbnailPath(), $"{fileName}.png")
                            )); // for megabytes
                    } // make a tutorial for the app in the app itself with hints and popups
                }
            }

            await _clipContext.SaveChangesAsync(); // maybe first save all of the video files and then go through and gradually generate the thumbnails for each and just edit the database object
            return allGameAmount;
        }

        public int GenerateAllThumbnails() // todo, make a selective thubmnail generation function so it doesn't have to go through everything to generate all of the thumbnails
        { // we need to have the thumbnail generation be a function in the wpf app, since I need to call StorageFile.GetThumbnailAsync(), something which I can only call when the wpf application is packaged as an msix package
            int totalGeneratedThumbnails = 0;
            /*
            if (!Directory.Exists("C:\\Users\\Nevin\\Desktop\\resync\\thumbnails"))
                Directory.CreateDirectory("C:\\Users\\Nevin\\Desktop\\resync\\thumbnails"); // todo: make this get from the config
            */
            foreach (var videoFile in _clipContext.VideoFiles.ToList())
            {
                if (!File.Exists(videoFile.ThumbnailLocation))
                {
                    totalGeneratedThumbnails++;
                    ShellFile shellFile = ShellFile.FromFilePath(videoFile.VideoLocation);
                    Bitmap shellThumb = shellFile.Thumbnail.LargeBitmap;
                    shellThumb.Save(videoFile.ThumbnailLocation); // could actually store it in the memory and then just return a list of all of the video files from here
                    // todo: make a thumbnail validation function which will check if all the files exist.
                }
            }

            return totalGeneratedThumbnails;
        }

        public async Task<List<VideoFile>> GetAllUserClips()
        {
            var allUserClips = await _clipContext.VideoFiles.ToListAsync();
            return allUserClips;
        }

        public async Task<List<string>> GetAllGames()
        {
            return await _clipContext.VideoFiles.Select(m => m.Game).Distinct().ToListAsync();
        }
    }
}
