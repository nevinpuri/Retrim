using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Resync_Edit.Events;
using SyncServiceLibrary.Interfaces;
using SyncServiceLibrary.Models;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Resync_Edit.ViewModels
{
    class MainMenuViewModel : BindableBase, INavigationAware
    {

        private IRegionManager _regionManager;

        private IEventAggregator _eventAggregator;

        private IUserConfigHelper _configHelper;

        private ISyncService _syncService;

        private bool _fromReset;

        private DelegateCommand _fileSelect;

        public DelegateCommand FileSelect => _fileSelect ??= new DelegateCommand(FileSelect_Execute);

        public MainMenuViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IUserConfigHelper configHelper, ISyncService syncService)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _configHelper = configHelper;
            _syncService = syncService;
            _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = false});
        }

        private async void FileSelect_Execute()
        {
            FolderBrowserDialog folderBrowser = new FolderBrowserDialog();
            folderBrowser.ShowNewFolderButton = true;
            folderBrowser.RootFolder = Environment.SpecialFolder.MyVideos;
            using (FolderBrowserDialog dialog = new FolderBrowserDialog())
            {
                folderBrowser.ShowNewFolderButton = true;
                folderBrowser.RootFolder = Environment.SpecialFolder.MyVideos;
                if (folderBrowser.ShowDialog() != DialogResult.OK) return;
                await _configHelper.SetInitialStart(folderBrowser.SelectedPath);
                await _syncService.ResetClips();
                var navigationParameters = new NavigationParameters();
                if (_fromReset)
                {
                    navigationParameters.Add("FromReset", true);
                }
                _regionManager.RequestNavigate("ContentRegion", "Library" + navigationParameters.ToString());
                // _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
            /*
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() != DialogResult.OK) return;
                filePath = fileDialog.FileName;
                var navigationParameters = new NavigationParameters
                {
                    { "UserVideos", fileDialog.FileName }
                };
                _regionManager.RequestNavigate("ContentRegion", new Uri("VideoPlayer" + navigationParameters.ToString(), UriKind.Relative));
                _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = true});
                // _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
            */
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            if (navigationContext.Parameters["FromReset"] != null)
            {
                _fromReset = true;
            }
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

    }
}
