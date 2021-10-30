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
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Prism.Ioc;
using Prism.Unity;
using Resync_Edit.Models;
using Resync_Edit.Views;
using SyncServiceLibrary;
using SyncServiceLibrary.Interfaces;
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
            UserConfigHelper configHelper = new UserConfigHelper();
            if (!configHelper.CheckUserConfig()) configHelper.CreateUserConfig();

            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            Unosquare.FFME.Library.FFmpegDirectory =
                Path.Join(currentDir, @"ffmpeg\bin");

            GlobalFFOptions.Configure(new FFOptions
            {
                BinaryFolder = 
                Path.Join(currentDir, @"ffmpeg\bin"),
                TemporaryFilesFolder = Path.Join(configHelper.GetTempPath())
            });

            if (!File.Exists(configHelper.GetConfigLocation()))
            {
                configHelper.CreateUserConfig();
            }

            var settings = JsonConvert.DeserializeObject<SettingsConfig>(File.ReadAllText(configHelper.GetConfigLocation()));
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
            UserConfigHelper configHelper = new UserConfigHelper();
            containerRegistry.Register<ISyncService, SyncService>();
            containerRegistry.Register<IUserConfigHelper, UserConfigHelper>();
            var optionsBuilder = new DbContextOptionsBuilder<ClipContext>();
            optionsBuilder.UseSqlite($"Data Source={configHelper.GetDbPath()}");
            containerRegistry.RegisterInstance(optionsBuilder.Options);
            containerRegistry.RegisterForNavigation<Library>();
            containerRegistry.RegisterForNavigation<MainMenu>();
            containerRegistry.RegisterForNavigation<VideoPlayer>();
            containerRegistry.RegisterForNavigation<Settings>();
            containerRegistry.RegisterForNavigation<MenuBar>();
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
