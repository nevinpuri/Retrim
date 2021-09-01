using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Resync_Edit.Events;

namespace Resync_Edit.ViewModels
{
    public class MenuBarViewModel : BindableBase
    {

        private string _currentlyLoadedVideo;

        private IRegionManager _regionManager;

        public string CurrentlyLoadedVideo
        {
            get => _currentlyLoadedVideo;
            set => SetProperty(ref _currentlyLoadedVideo, value);
        }

        private DelegateCommand _previousNavigate;
        public DelegateCommand PreviousNavigate => _previousNavigate ??= new DelegateCommand(PreviousNavigate_Execute);

        public MenuBarViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            eventAggregator.GetEvent<VideoPlayerEvent>().Subscribe(s => CurrentlyLoadedVideo = s);
        }

        private async void PreviousNavigate_Execute()
        {
            _regionManager.RequestNavigate("ContentRegion", "MainMenu");
            //_regionManager.Deactivate("MenuBar");
        }
    }
}
