using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using Prism.Mvvm;
using Unosquare.FFME;
using Unosquare.FFME.Common;

namespace Resync_Edit.Models
{
    public class ResyncPlayer : BindableBase
    {
        private MediaElement _mediaElement;

        /// <summary>
        /// Main Resync Media Element
        /// </summary>
        public MediaElement MediaElement
        {
            get => _mediaElement;
            set => SetProperty(ref _mediaElement, value);
        }


        private double _duration;

        /// <summary>
        /// Duration of the MediaElement
        /// </summary>
        public double Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private double _seekPosition;

        /// <summary>
        /// The Current position of the MediaElement
        /// </summary>
        public double SeekPosition
        {
            get => _seekPosition;
            set => SetProperty(ref _seekPosition, value);
        }

        private double _volume;

        public double Volume
        {
            get => _volume;
            set => SetProperty(ref _volume, value);
        }

        public ResyncPlayer()
        {
            MediaElement = new MediaElement();
            MediaElement.Loaded += MediaElementOnLoaded;
        }

        private void MediaElementOnLoaded(object sender, RoutedEventArgs e)
        {
            MediaElement.LoadedBehavior = MediaPlaybackState.Manual;
            MediaElement.UnloadedBehavior = MediaPlaybackState.Manual;
            MediaElement.LoopingBehavior = MediaPlaybackState.Play;
            MediaElement.ScrubbingEnabled = true;
            MediaElement.MediaOpened -= MediaElementOnMediaOpened;
            MediaElement.PositionChanged -= MediaElementOnPositionChanged;
            MediaElement.MediaOpened += MediaElementOnMediaOpened;
            MediaElement.PositionChanged += MediaElementOnPositionChanged;
        }

        private void MediaElementOnPositionChanged(object? sender, PositionChangedEventArgs e)
        {
            SeekPosition = e.Position.TotalSeconds;
        }

        private void MediaElementOnMediaOpened(object? sender, MediaOpenedEventArgs e)
        {
            Duration = e.Info.Duration.TotalSeconds;
        }

        public async Task Play() => await MediaElement.Play();

        public async Task Pause() => await MediaElement.Pause();

        public async Task Close() => await MediaElement.Close();

        public async Task Seek(TimeSpan time)
        {
            await MediaElement.Seek(time);
        }
    }
}
