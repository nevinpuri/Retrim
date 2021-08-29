using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Mime;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using FFMpegCore;
using FFMpegCore.Enums;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
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

        private IRegionManager _regionManager;

        private string _currentVideo;

        private MediaElement _mediaElement;

        private double _minThumb;

        private double _maxThumb = 750;

        private double _selectionStart;

        private double _selectionEnd;

        private double _duration;

        private TimeSpan _currentTime;

        private double _seekPosition;

        private bool _exporting;

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

        public MediaElement MediaElement
        {
            get => _mediaElement;
            set => SetProperty(ref _mediaElement, value);
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
            set => SetProperty(ref _currentTime, value);
        }


        public double SeekPosition
        {
            get => _seekPosition;
            set => SetProperty(ref _seekPosition, value);
        }

        public bool Exporting
        {
            get => _exporting;
            set => SetProperty(ref _exporting, value);
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

        /*
        private DelegateCommand _mediaLoadCommand;
        */

        /*
        private DelegateCommand<MediaOpenedEventArgs> _mediaOpenedCommand;
        */

        private DelegateCommand _sliderDragStartCommand;

        private DelegateCommand _sliderDragEndCommand;

        /*
        private DelegateCommand<PositionChangedEventArgs> _positionChangedCommand;
        */

        private DelegateCommand _playPauseToggleCommand;

        private DelegateCommand _muteCommand;

        private DelegateCommand _previousNavigate;

        private DelegateCommand _saveCopyCommand;

        private DelegateCommand _saveCommand;

        public DelegateCommand PlayRequestedCommand =>
            _playRequestedCommand ??= new DelegateCommand(PlayRequested_Execute);

        public DelegateCommand PauseRequestedCommand =>
            _pauseRequestedCommand ??= new DelegateCommand(PauseRequested_Execute);

        public DelegateCommand CloseRequestedCommand =>
            _closeRequestedCommand ??= new DelegateCommand(CloseRequested_Execute);

        public DelegateCommand<RoutedPropertyChangedEventArgs<double>> VolumeChangedCommand =>
            _volumeChangedCommand ??= new DelegateCommand<RoutedPropertyChangedEventArgs<double>>(VolumeChanged_Execute);

        /*
        public DelegateCommand MediaLoadCommand => _mediaLoadCommand ??= new DelegateCommand(MediaLoad_Execute);
        */

        public DelegateCommand<DragDeltaEventArgs> MinThumbChangedCommand => _minThumbChangedCommand ??=
            new DelegateCommand<DragDeltaEventArgs>(MinThumbChanged_Execute);

        public DelegateCommand<DragDeltaEventArgs> MaxThumbChangedCommand => _maxThumbChangedCommand ??=
            new DelegateCommand<DragDeltaEventArgs>(MaxThumbChanged_Execute);

        /*
        public DelegateCommand<MediaOpenedEventArgs> MediaOpenedCommand => _mediaOpenedCommand ??=
            new DelegateCommand<MediaOpenedEventArgs>(MediaOpened_Execute);
        */

        public DelegateCommand SliderDragStartCommand =>
            _sliderDragStartCommand ??= new DelegateCommand(SliderDragStart_Execute);

        public DelegateCommand SliderDragEndCommand =>
            _sliderDragEndCommand ??= new DelegateCommand(SliderDragEnd_Execute);

        /*
        public DelegateCommand<PositionChangedEventArgs> PositionChangedCommand => _positionChangedCommand ??=
            new DelegateCommand<PositionChangedEventArgs>(PositionChanged_Execute);
        */

        public DelegateCommand PlayPauseToggleCommand =>
            _playPauseToggleCommand ??= new DelegateCommand(PlayPauseToggle_Execute);
        public DelegateCommand MuteCommand => _muteCommand ??= new DelegateCommand(MuteCommand_Execute);

        public DelegateCommand PreviousNavigate => _previousNavigate ??= new DelegateCommand(PreviousNavigate_Execute);

        public DelegateCommand SaveCopyCommand => _saveCopyCommand ??= new DelegateCommand(SaveCopy_Execute);

        public DelegateCommand SaveCommand => _saveCommand ??= new DelegateCommand(SaveVideo_Execute);

        private async void PlayRequested_Execute()
        {
            Play = false;
            Pause = true;
            await MediaElement.Play();
            // PlayRequested(this, EventArgs.Empty);
        }

        private async void PauseRequested_Execute()
        {
            Play = true;
            Pause = false;
            await MediaElement.Pause();
            // PauseRequested(this, EventArgs.Empty);
        }

        private async void CloseRequested_Execute()
        {
            await MediaElement.Close();
            // CloseRequested?.Invoke(this, EventArgs.Empty);
        }

        private void VolumeChanged_Execute(RoutedPropertyChangedEventArgs<double> e)
        {
            Volume = e.NewValue;
            MediaElement.Volume = e.NewValue;
            // VolumeChangeRequested?.Invoke(this, new VolumeEventArgs(e.NewValue));
        }

        private async void MediaLoad_Execute(object sender, RoutedEventArgs e)
        {
            MediaElement.LoadedBehavior = MediaPlaybackState.Manual;
            MediaElement.UnloadedBehavior = MediaPlaybackState.Manual;
            MediaElement.LoopingBehavior = MediaPlaybackState.Play;
            MediaElement.ScrubbingEnabled = true;
            MediaElement.MediaOpened -= MediaOpened_Execute;
            MediaElement.PositionChanged -= PositionChanged_Execute;
            MediaElement.MediaOpened += MediaOpened_Execute;
            MediaElement.PositionChanged += PositionChanged_Execute;
        }

        private async void MinThumbChanged_Execute(DragDeltaEventArgs e)
        {
            if (MinThumb + e.HorizontalChange < MaxThumb && MinThumb + e.HorizontalChange > 0)
            {
                MinThumb += e.HorizontalChange;
                // MinThumbChangeRequested?.Invoke(this, new SliderEventArgs(MinThumb + e.HorizontalChange));
                if (Math.Round(SeekPosition, 1) != Math.Round(SelectionStart, 1))
                {
                    CurrentTime = TimeSpan.FromSeconds(SelectionStart);
                    SeekPosition = SelectionStart;
                    await MediaElement.Seek(TimeSpan.FromSeconds(SeekPosition));
                    // SeekChangeRequested?.Invoke(this, new SliderEventArgs(SeekPosition));
                }
                SelectionStart = (MinThumb + e.HorizontalChange) / 750 * Duration;
            }
        }

        private async void MaxThumbChanged_Execute(DragDeltaEventArgs e)
        {
            if (MaxThumb + e.HorizontalChange > MinThumb && MaxThumb + e.HorizontalChange < 747)
            {
                MaxThumb += e.HorizontalChange;
                if (Math.Round(SeekPosition, 1) != Math.Round(SelectionEnd, 1))
                {
                    CurrentTime = TimeSpan.FromSeconds(SelectionEnd);
                    SeekPosition = SelectionEnd;
                    await MediaElement.Seek(TimeSpan.FromSeconds(SeekPosition));
                    // SeekChangeRequested?.Invoke(this, new SliderEventArgs(SeekPosition));
                }
                // MaxThumbChangeRequested?.Invoke(this, new SliderEventArgs(MaxThumb + e.HorizontalChange));
                SelectionEnd = (MaxThumb + e.HorizontalChange) / 750 * Duration;
            }
        }

        private void MediaOpened_Execute(object sender, MediaOpenedEventArgs e)
        {
            Duration = e.Info.Duration.TotalSeconds;
        }

        private async void SliderDragStart_Execute()
        {
            await MediaElement.Pause();
            CurrentTime = TimeSpan.FromSeconds(SeekPosition);
            await MediaElement.Seek(TimeSpan.FromSeconds(SeekPosition));
            // PauseRequested?.Invoke(this, EventArgs.Empty);
        }

        private async void SliderDragEnd_Execute()
        {
            // CurrentTime = TimeSpan.FromSeconds(SeekPosition);
            CurrentTime = TimeSpan.FromSeconds(SeekPosition);
            await MediaElement.Seek(TimeSpan.FromSeconds(SeekPosition));
            if (Pause) await MediaElement.Play();
            // MainSeekRequested?.Invoke(this, new SeekEventArgs(SeekPosition, Pause));
        }

        public void PositionChanged_Execute(object sender, PositionChangedEventArgs e)
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

        private async void PreviousNavigate_Execute()
        {
            await MediaElement.Close();
            _regionManager.RequestNavigate("ContentRegion", "MainMenu");
        }

        private async void SaveCopy_Execute()
        {
            SaveFileDialog fileSave = new SaveFileDialog();
            fileSave.Title = "Select Location to Save Copy";
            fileSave.ShowDialog();

            if (fileSave.FileName != "")
            {
                Exporting = true;
                await FFMpegArguments.FromFileInput(CurrentVideo, true, options => options
                        .UsingMultithreading(true)
                        .Seek(TimeSpan.FromSeconds(SelectionStart))
                        .WithDuration(TimeSpan.FromSeconds(SelectionEnd)))
                    .OutputToFile(fileSave.FileName, true, options => options
                        .WithCustomArgument("-c copy")
                        .WithFastStart())
                    .ProcessAsynchronously();
                Exporting = false;
                new ToastContentBuilder().AddText("Your video has finished converting!").Show();
                MessageBox.Show("done");
            }
        }

        private void SaveVideo_Execute()
        {
        }

        public VideoPlayerViewModel(IRegionManager regionManager)
        {
            MediaElement = new MediaElement();
            Volume = 1;
            _regionManager = regionManager;
            MediaElement.Loaded += MediaLoad_Execute;
        }

        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            VideoLocation = (string)navigationContext.Parameters["UserVideos"];
            CurrentVideo = VideoLocation;
            if (!MediaElement.IsLoaded)
            {
                await MediaElement.Close();
            }
            await MediaElement.Open(new Uri(CurrentVideo));
            await MediaElement.Play();
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
