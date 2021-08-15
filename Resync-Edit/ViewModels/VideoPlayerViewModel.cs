using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ABI.Windows.Media.Core;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Resync_Edit.ViewModels
{
    class VideoPlayerViewModel : BindableBase, INavigationAware
    {
        private string _title;

        private string _videoLocation;

        private bool _play = false;

        private bool _pause = true;

        private string _currentVideo;

        public bool Play
        {
            get => _play;
            set => SetProperty(ref _play, value);
        }

        public bool Pause
        {
            get => _pause;
            set => SetProperty(ref _pause, value);
        }

        public string VideoLocation
        {
            get => _videoLocation;
            set => SetProperty(ref _videoLocation, value);
        }

        public string CurrentVideo
        {
            get => _currentVideo;
            set => SetProperty(ref _currentVideo, value);
        }

        public event EventHandler PlayRequested;

        public event EventHandler PauseRequested;

        public event EventHandler CloseRequested;

        private DelegateCommand _playRequestedCommand;

        private DelegateCommand _pauseRequestedCommand;

        private DelegateCommand _closeRequestedCommand;

        public DelegateCommand PlayRequestedCommand =>
            _playRequestedCommand ??= new DelegateCommand(PlayRequested_Execute);

        public DelegateCommand PauseRequestedCommand =>
            _pauseRequestedCommand ??= new DelegateCommand(PauseRequested_Execute);

        public DelegateCommand CloseRequestedCommand =>
            _closeRequestedCommand ??= new DelegateCommand(CloseRequested_Execute);

        private void PlayRequested_Execute()
        {
            if (!(PlayRequested is null))
            {
                Play = false;
                Pause = true;
                PlayRequested(this, EventArgs.Empty);
            }
        }

        private void PauseRequested_Execute()
        {
            if (!(PauseRequested is null))
            {
                Play = true;
                Pause = false;
                PauseRequested(this, EventArgs.Empty);
            }
        }

        private void CloseRequested_Execute()
        {
            if (!(PauseRequested is null))
            {
                CloseRequested(this, EventArgs.Empty);
            }
        }

        public VideoPlayerViewModel()
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            VideoLocation = (string) navigationContext.Parameters["UserVideos"];
            CurrentVideo = VideoLocation;
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true;
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }


        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}

/*
 *
 *
            if (!(VideoSettings.VideoPath is null))
            {
                await Media.Open(new Uri(VideoSettings.VideoPath));
                Media.Volume = 0.1;
            }
 */
