using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Resync_Edit.ViewModels;
using Unosquare.FFME.Common;

namespace Resync_Edit.Views
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl, IMediaService
    {
        // VideoPlayerViewModel vm = new VideoPlayerViewModel();
        public VideoPlayer()
        {
            InitializeComponent();
            MyVideoPlayer.RendererOptions.UseLegacyAudioOut = true;
        }

        async void IMediaService.Play()
        {
            await MyVideoPlayer.Play();
        }

        async void IMediaService.Pause()
        {
            await MyVideoPlayer.Pause();
        }

        async void IMediaService.Stop()
        {
            await MyVideoPlayer.Stop();
        }

        async void IMediaService.LoadMedia(Uri mediaSrc)
        {
            await MyVideoPlayer.Open(mediaSrc);
            await MyVideoPlayer.Play();
        }

        async void IMediaService.SetVolume(double volume)
        {
            MyVideoPlayer.Volume = volume;
        }

        private void VideoPlayer_OnLoaded(object sender, RoutedEventArgs e)
        {
            Focus();
        }
    }
}
