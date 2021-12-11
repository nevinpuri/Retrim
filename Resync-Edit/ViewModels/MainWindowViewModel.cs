using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Resync_Edit.Events;
using Resync_Edit.Views;

namespace Resync_Edit.ViewModels
{
    class MainWindowViewModel : BindableBase
    {
        private readonly IRegionManager _regionManager;

        private readonly IEventAggregator _eventAggregator;

        private DelegateCommand _mainWindowLoaded;

        public DelegateCommand MainWindowLoaded => _mainWindowLoaded ??= new DelegateCommand(MainWindow_LoadExecute);

        public MainWindowViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _eventAggregator.GetEvent<MenuBarEvent>().Subscribe(OnChangeMenuBar);
        }

        private void OnChangeMenuBar(MenuBarEventArgs e)
        {
            if (e.Open)
            {
                MenuBar menuBar = new()
                {
                    Name = "MenuBar"
                };
                _regionManager.Regions["MenuRegion"].Add(menuBar, "MenuBar");
                _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
            else
            {
                var menuBar = _regionManager.Regions["MenuRegion"].Views;
                if (!menuBar.Any()) return;
                _regionManager.Regions["MenuRegion"].Remove(menuBar.First()); // this is so hacky
            }
        }

        private void MainWindow_LoadExecute()
        {
            string[] commandLineArgs = Environment.GetCommandLineArgs(); // [a-zA-Z]:[\\\/](?:[a-zA-Z0-9]+[\\\/])*([a-zA-Z0-9]+.*)
            commandLineArgs = commandLineArgs.Skip(1).ToArray(); // to only get videos
            if (commandLineArgs.Length < 1)
            {
                _regionManager.RequestNavigate("ContentRegion", "Library");
            }
            else
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("UserVideos", commandLineArgs[0]);
                _regionManager.RequestNavigate("ContentRegion", new Uri("VideoPlayer" + navigationParameters.ToString(), UriKind.Relative));
                _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = true});
                // _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
        }
    }
}
