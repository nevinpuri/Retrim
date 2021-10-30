using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using SyncServiceLibrary;
using ModernWpf.Controls;
using System.Windows.Threading;
using Microsoft.Extensions.DependencyInjection;
using ModernWpf.Navigation;
using SyncServiceLibrary.Interfaces;
using Frame = ModernWpf.Controls.Frame;
using NavigationEventArgs = System.Windows.Navigation.NavigationEventArgs;

namespace Resync_Edit.Views
{
    /// <summary>
    /// Interaction logic for Library.xaml
    /// </summary>
    public partial class Library : UserControl
    {
        // public ObservableCollection<VideoFile> Images = new ObservableCollection<VideoFile>();

        public Library()
        {
            InitializeComponent();
        }

        /*
        private async void Button_Click(object sender, MouseButtonEventArgs e)
        {
            TogglePanel();
            var item = sender as StackPanel; // there's got to be a better way to do this
            if (item == null) throw new NullReferenceException();
            var videoImage = item.DataContext as VideoFile;
            if (videoImage == null) throw new NullReferenceException();
            _contentFrame.Navigate(typeof(VideoPlayer), new VideoPlayerParameters(_contentFrame, videoImage));
            MediaPlayerElement.Source = new Uri(videoImage.VideoLocation); // maybe I will use xaml islands windows media player... what are you going to do about it?
            MediaPlayerElement.Play();
        }
    */


        /*
        private async Task PopulateVideosAsync()
        {
            try
            {
                int syncCount = await _syncService.QueryAllVideos();
                List<VideoFile>
                    clips = await _webUserApi.GetAllClips(); // add a check to see if the clips have a thumbnail
                await _syncService.GenerateAllThumbnails(); // kind of bad code
                foreach (var clip in clips) // maybe using a cancellation token on the async methods can help me fix the bug
                {
                    if (!File.Exists(clip.ThumbnailLocation))
                    {
                        clip.ThumbnailLocation = "C:/Users/Nevin/Desktop/IMG_20210116_171559.jpg"; // add a right click context menu which sais "open file location" to file
                    }
                }

                ObservableCollection<VideoFile> clipItems = new ObservableCollection<VideoFile>(clips);

                Dispatcher.Invoke(
                    () => // just do a dispatcher.invoke for each clip adding it to the observable collection, no need to create a new one
                    {
                        Images = clipItems;
                        LoadingPanel.Visibility = Visibility.Collapsed;
                    });

            }
            catch (HttpRequestException exception)
            {
                MessageBox.Show($"Error: {exception.Message}");
            }

            catch (NullReferenceException exception)
            {
                MessageBox.Show($"Error: {exception.Message}"); // try to log it to the database and then periodically send the database info as bug reports
            }

        }
*/

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            /*
            await Task.Run(async () => await PopulateVideosAsync());
            ImagesBox.ItemsSource = Images; // make sure you put the loading circle and just make it hidden or collapsed
            */
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        // protected override void OnNavigatedTo(NavigationEventArgs e) // this breaks because it's not being navigated to from the home directory, but the back button navigation is screwing up the parameters
        // {
            /*
            var parameter = e.Parameter() as LibraryParameters;
            _webUserApi = parameter._webUserApi;
            _syncService = parameter._syncService;
            _contentFrame = parameter._contentFrame;
            */
        // }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
         //   _contentFrame.Navigate(typeof(Home));
        }
    }
}