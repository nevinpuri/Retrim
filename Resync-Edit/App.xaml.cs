using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
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
            Unosquare.FFME.Library.FFmpegDirectory =
                @"C:\Users\Nevin\source\repos\Resync-Edit\Resync-Edit\bin\Debug\net5.0-windows\ffmpeg\bin";
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            containerRegistry.RegisterForNavigation<MainMenu>();
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
