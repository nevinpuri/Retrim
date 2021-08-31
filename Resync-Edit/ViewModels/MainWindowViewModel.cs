using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Resync_Edit.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private DelegateCommand _mainWindowLoaded;

        public DelegateCommand MainWindowLoaded => _mainWindowLoaded ??= new DelegateCommand(MainWindow_LoadExecute);

        public MainWindowViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void MainWindow_LoadExecute()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs(); // [a-zA-Z]:[\\\/](?:[a-zA-Z0-9]+[\\\/])*([a-zA-Z0-9]+.*)
            commandLineArgs = commandLineArgs.Skip(1).ToArray(); // to only get videos
            if (commandLineArgs.Length < 1)
                _regionManager.RequestNavigate("ContentRegion", "MainMenu");
            else
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("UserVideos", commandLineArgs[0]);
                _regionManager.RequestNavigate("ContentRegion", new Uri("VideoPlayer" + navigationParameters.ToString(), UriKind.Relative));
                _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
        }
    }
}
