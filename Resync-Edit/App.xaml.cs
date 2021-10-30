using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using FFMpegCore;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Unity;
using Resync_Edit.Models;
using Resync_Edit.Views;
using Unity;

namespace Resync_Edit
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : PrismApplication
    {
        public App()
        {
            if (!Directory.Exists(Path.Join(Path.GetTempPath(), "Resync-Temp")))
                Directory.CreateDirectory(Path.Join(Path.GetTempPath(), "Resync-Temp"));

            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Unosquare.FFME.Library.FFmpegDirectory =
                Path.Join(currentDir, @"ffmpeg\bin");

            GlobalFFOptions.Configure(new FFOptions
            {
                BinaryFolder = 
                Path.Join(currentDir, @"ffmpeg\bin"),
                TemporaryFilesFolder = Path.Join(Path.GetTempPath(), "Resync-Temp")
            });

            if (!File.Exists(Path.Join(currentDir, "config.json")))
            {
                File.WriteAllText(Path.Join(currentDir, "config.json"), "{\"CheckForUpdates\": true, \"UpdateServer\": \"https://nevin.cc/resync/update\", \"CompressVideos\": false}");
            }

            var settings = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(Path.Join(currentDir, "config.json")));
            if (settings is null)
            {
                File.Delete(Path.Join(currentDir, "config.json"));
                MessageBox.Show("Error: Failed to Parse Config File");
                return;
            }
            Config.CheckForUpdates = settings.CheckForUpdates;
            Config.CompressVideos = settings.CompressVideos;
            Config.UpdateServer = settings.UpdateServer;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainMenu>();
            containerRegistry.RegisterForNavigation<VideoPlayer>();
            containerRegistry.RegisterForNavigation<Settings>();
            containerRegistry.RegisterForNavigation<MenuBar>();
            containerRegistry.RegisterForNavigation<Library>();
        }

        protected override Window CreateShell()
        {
            var w = Container.Resolve<MainWindow>();
            return w;
        }

        private void PrismApplication_Startup(object sender, StartupEventArgs e)
        {
        }
    }
}
