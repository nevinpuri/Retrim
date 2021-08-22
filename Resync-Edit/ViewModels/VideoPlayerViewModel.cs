using System;
using System.Windows;
using System.Windows.Controls.Primitives;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using Unosquare.FFME;
using Unosquare.FFME.Common;

namespace Resync_Edit.ViewModels
{
    class VideoPlayerViewModel : BindableBase, INavigationAware
    {
        private string _title;

        private string _videoLocation;

        private bool _play;

        private bool _pause = true;

        private double _volume;

        private string _currentVideo;

        private double _minThumb;

        private double _maxThumb = 750;

        private double _selectionStart;

        private double _selectionEnd;

        private double _duration;

        private TimeSpan _currentTime;

        private double _seekPosition;

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

        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
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

        public double MinThumb
        {
            get => _minThumb;
            set => SetProperty(ref _minThumb, value);
        }

        public double MaxThumb
        {
            get => _maxThumb;
            set => SetProperty(ref _maxThumb, value);
        }

        public double SelectionStart
        {
            get => _selectionStart;
            set => SetProperty(ref _selectionStart, value);
        }

        public double SelectionEnd
        {
            get => _selectionEnd;
            set => SetProperty(ref _selectionEnd, value);
        }

        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        public TimeSpan CurrentTime
        {
            get => _currentTime;
            set
            {
                SetProperty(ref _currentTime, value);
            }
        }


        public double SeekPosition
        {
            get => _seekPosition;
            set
            {
                SetProperty(ref _seekPosition, value);
            }
        }

        public event EventHandler PlayRequested;

        public event EventHandler PauseRequested;

        public event EventHandler CloseRequested;

        public event EventHandler<VolumeEventArgs> VolumeChangeRequested;

        public event EventHandler<SliderEventArgs> SeekChangeRequested;

        public event EventHandler<SeekEventArgs> MainSeekRequested;

        private DelegateCommand _playRequestedCommand;

        private DelegateCommand _pauseRequestedCommand;

        private DelegateCommand _closeRequestedCommand;

        private DelegateCommand<DragDeltaEventArgs> _minThumbChangedCommand;

        private DelegateCommand<DragDeltaEventArgs> _maxThumbChangedCommand;

        private DelegateCommand<RoutedPropertyChangedEventArgs<double>> _volumeChangedCommand;

        private DelegateCommand<MediaOpenedEventArgs> _mediaOpenedCommand;

        private DelegateCommand _sliderDragStartCommand;

        private DelegateCommand _sliderDragEndCommand;

        private DelegateCommand<PositionChangedEventArgs> _positionChangedCommand;

        private DelegateCommand _playPauseToggleCommand;

        private DelegateCommand _muteCommand;

        private DelegateCommand _previousNavigate;

        public DelegateCommand PlayRequestedCommand =>
            _playRequestedCommand ??= new DelegateCommand(PlayRequested_Execute);

        public DelegateCommand PauseRequestedCommand =>
            _pauseRequestedCommand ??= new DelegateCommand(PauseRequested_Execute);

        public DelegateCommand CloseRequestedCommand =>
            _closeRequestedCommand ??= new DelegateCommand(CloseRequested_Execute);

        public DelegateCommand<RoutedPropertyChangedEventArgs<double>> VolumeChangedCommand =>
            _volumeChangedCommand ??= new DelegateCommand<RoutedPropertyChangedEventArgs<double>>(VolumeChanged_Execute);

        public DelegateCommand<DragDeltaEventArgs> MinThumbChangedCommand => _minThumbChangedCommand ??=
            new DelegateCommand<DragDeltaEventArgs>(MinThumbChanged_Execute);

        public DelegateCommand<DragDeltaEventArgs> MaxThumbChangedCommand => _maxThumbChangedCommand ??=
            new DelegateCommand<DragDeltaEventArgs>(MaxThumbChanged_Execute);

        public DelegateCommand<MediaOpenedEventArgs> MediaOpenedCommand => _mediaOpenedCommand ??=
            new DelegateCommand<MediaOpenedEventArgs>(MediaOpened_Execute);

        public DelegateCommand SliderDragStartCommand =>
            _sliderDragStartCommand ??= new DelegateCommand(SliderDragStart_Execute);

        public DelegateCommand SliderDragEndCommand =>
            _sliderDragEndCommand ??= new DelegateCommand(SliderDragEnd_Execute);

        public DelegateCommand<PositionChangedEventArgs> PositionChangedCommand => _positionChangedCommand ??=
            new DelegateCommand<PositionChangedEventArgs>(PositionChanged_Execute);

        public DelegateCommand PlayPauseToggleCommand =>
            _playPauseToggleCommand ??= new DelegateCommand(PlayPauseToggle_Execute);
        public DelegateCommand MuteCommand => _muteCommand ??= new DelegateCommand(MuteCommand_Execute);

        public DelegateCommand PreviousNavigate => _previousNavigate ??= new DelegateCommand(PreviousNavigate_Execute);

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
            CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void VolumeChanged_Execute(RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = e.NewValue;
            VolumeChangeRequested?.Invoke(this, new VolumeEventArgs(e.NewValue));
        }

        private void MinThumbChanged_Execute(DragDeltaEventArgs e)
        {
            if (MinThumb + e.HorizontalChange < MaxThumb && MinThumb + e.HorizontalChange > 0)
            {
                MinThumb += e.HorizontalChange;
                // MinThumbChangeRequested?.Invoke(this, new SliderEventArgs(MinThumb + e.HorizontalChange));
                if (Math.Round(SeekPosition, 1) != Math.Round(SelectionStart, 1))
                {
                    CurrentTime = TimeSpan.FromSeconds(SelectionStart);
                    SeekPosition = SelectionStart;
                    SeekChangeRequested?.Invoke(this, new SliderEventArgs(SeekPosition));
                }
                SelectionStart = (MinThumb + e.HorizontalChange) / 750 * Duration;
            }
        }

        private void MaxThumbChanged_Execute(DragDeltaEventArgs e)
        {
            if (MaxThumb + e.HorizontalChange > MinThumb && MaxThumb + e.HorizontalChange < 750)
            {
                MaxThumb += e.HorizontalChange;
                if (Math.Round(SeekPosition, 1) != Math.Round(SelectionEnd, 1))
                {
                    CurrentTime = TimeSpan.FromSeconds(SelectionEnd);
                    SeekPosition = SelectionEnd;
                    SeekChangeRequested?.Invoke(this, new SliderEventArgs(SeekPosition));
                }
                // MaxThumbChangeRequested?.Invoke(this, new SliderEventArgs(MaxThumb + e.HorizontalChange));
                SelectionEnd = (MaxThumb + e.HorizontalChange) / 750 * Duration;
            }
        }

        private void MediaOpened_Execute(MediaOpenedEventArgs e)
        {
            Duration = e.Info.Duration.TotalSeconds;
        }

        private void SliderDragStart_Execute()
        {
            PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void SliderDragEnd_Execute()
        {
            // CurrentTime = TimeSpan.FromSeconds(SeekPosition);
            CurrentTime = TimeSpan.FromSeconds(SeekPosition);
            MainSeekRequested?.Invoke(this, new SeekEventArgs(SeekPosition, Pause));
        }

        public void PositionChanged_Execute(PositionChangedEventArgs e)
        {
            SeekPosition = e.Position.TotalSeconds;
        }

        private void PlayPauseToggle_Execute()
        {
            if (Play) PlayRequested_Execute();
            else PauseRequested_Execute();
        }

        private void MuteCommand_Execute()
        {
            if (Volume > 0) Volume = 0;
            else Volume = 1;
        }

        private void PreviousNavigate_Execute()
        {
        }

        public VideoPlayerViewModel()
        {
            Volume = 1;
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
