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
using SyncServiceLibrary.Models;

namespace Resync_Edit.ViewModels
{
    public class SettingsViewModel : BindableBase
    {

        private IRegionManager _regionManager;

        private IEventAggregator _eventAggregator;

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
            UserConfigHelper configHelper = new UserConfigHelper();
            await configHelper.SetUserConfig(new SettingConfig()
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
            UpdateServer = "https://nevin.cc/resync/update";
        }

        public SettingsViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
        }
    }
}
