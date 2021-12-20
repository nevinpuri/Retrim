using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Resync_Edit.Events;
using Resync_Edit.Models;
using SyncServiceLibrary;
using SyncServiceLibrary.Interfaces;
using SyncServiceLibrary.Models;

namespace Resync_Edit.ViewModels
{
    public class SettingsViewModel : BindableBase
    {

        private IRegionManager _regionManager;

        private IEventAggregator _eventAggregator;

        private IUserConfigHelper _configHelper;

        private bool _checkForUpdates;

        public bool CheckForUpdates
        {
            get => _checkForUpdates;
            set => SetProperty(ref _checkForUpdates, value);
        }

        private string _updateServer;

        public string UpdateServer
        {
            get => _updateServer;
            set => SetProperty(ref _updateServer, value);
        }

        private bool _encodeVideos;

        public bool EncodeVideos
        {
            get => _encodeVideos;
            set => SetProperty(ref _encodeVideos, value);
        }

        private DelegateCommand _resetServerCommand;

        public DelegateCommand ResetServerCommand => _resetServerCommand ??= new DelegateCommand(ResetServer_Execute);

        private DelegateCommand _loadCommand;

        public DelegateCommand LoadCommand => _loadCommand ??= new DelegateCommand(Load_Execute);

        private void Load_Execute()
        {
            var config = _configHelper.GetUserConfig();
            CheckForUpdates = config.CheckForUpdates;
            UpdateServer = config.UpdateServer;
            EncodeVideos = config.CompressVideos;
        }

        private DelegateCommand _resetVideoLocationCommand;

        public DelegateCommand ResetVideoLocationCommand =>
            _resetVideoLocationCommand ??= new DelegateCommand(ResetVideoLocation_Execute);

        private void ResetVideoLocation_Execute()
        {
            var navigationParameters = new NavigationParameters();
            navigationParameters.Add("FromReset", "true");
            _regionManager.RequestNavigate("ContentRegion", "MainMenu" + navigationParameters.ToString());
        }

        private DelegateCommand _cancelChangesCommand;

        public DelegateCommand CancelChangesCommand =>
            _cancelChangesCommand ??= new DelegateCommand(CancelChangesCommand_Execute);


        private DelegateCommand _applyChangesCommand;

        public DelegateCommand ApplyChangesCommand =>
            _applyChangesCommand ??= new DelegateCommand(ApplyChanges_Execute);

        private void CancelChangesCommand_Execute()
        {
            _regionManager.RequestNavigate("ContentRegion", "Library");
            // _regionManager.RequestNavigate("MenuRegion", "MainMenu");
            _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = false});
        }

        private async void ApplyChanges_Execute()
        {
            string resyncPath = Path.Join(Path.GetTempPath(), "resync");
            await _configHelper.SetUserConfig(new SettingConfig()
            {
                CheckForUpdates = CheckForUpdates,
                CompressVideos = EncodeVideos,
                FolderLocation = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos),
                UpdateServer = UpdateServer,
                ThumbnailLocation = Path.Join(resyncPath, "thumbnails"),
                InitialStart = false
            });
            _regionManager.RequestNavigate("ContentRegion", "Library");
            // _regionManager.RequestNavigate("MenuRegion", "MainMenu");
            _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = false});
        }

        private void ResetServer_Execute()
        {
            UpdateServer = "https://resync.to/update";
        }

        public SettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator, IUserConfigHelper configHelper)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            _configHelper = configHelper;
        }
    }
}
