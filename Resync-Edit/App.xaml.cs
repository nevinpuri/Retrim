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
using Prism.Ioc;
using Prism.Unity;
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
            Unosquare.FFME.Library.FFmpegDirectory =
                Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ffmpeg\bin");
                // @"C:\Users\Nevin\source\repos\Resync-Edit\Resync-Edit\bin\Debug\net5.0-windows10.0.19041.0\ffmpeg\bin";
            GlobalFFOptions.Configure(new FFOptions
            {
                BinaryFolder = 
                Path.Join(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"ffmpeg\bin"),
                 //   @"C:\Users\Nevin\source\repos\Resync-Edit\Resync-Edit\bin\Debug\net5.0-windows10.0.19041.0\ffmpeg\bin",
                TemporaryFilesFolder = Path.Join(Path.GetTempPath(), "Resync-Temp")
            });
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainMenu>();
            containerRegistry.RegisterForNavigation<VideoPlayer>();
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
