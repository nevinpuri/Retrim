using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Resync_Edit.Events;
using SyncServiceLibrary;
using SyncServiceLibrary.Interfaces;
using WinRT.Interop;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using MessageBox = System.Windows.MessageBox;

namespace Resync_Edit.ViewModels
{
    // docs.resadable.to/asdgisajdoig
    public class LibraryViewModel : BindableBase, INavigationAware, IConfirmNavigationRequest
    {

        private string _searchFilter;

        public string SearchFilter
        {
            get => _searchFilter;
            set => SetProperty(ref _searchFilter, value);
        }

        private ISyncService _syncService;

        private IEventAggregator _eventAggregator;

        private Task getVideosTask;

        private IRegionManager _regionManager;

        private IUserConfigHelper _configHelper;

        private VideoFile _selectedFile = null;

        private DelegateCommand _searchCommand;

        public DelegateCommand SearchCommand => _searchCommand ??= new DelegateCommand(SearchCommand_Execute);

        private async void SearchCommand_Execute()
        {
            if (SearchFilter == null) SearchFilter = "";
            List<VideoFile> clips = await GetAllClips(SearchFilter);
            Images = new ObservableCollection<VideoFile>(clips);
        }

        public VideoFile SelectedFile
        {
            get => _selectedFile;
            set => SetProperty<VideoFile>(ref _selectedFile, value);
        }

        private ItemCollection _userFilter;

        public ItemCollection UserFilter
        {
            get => _userFilter;
            set => SetProperty(ref _userFilter, value);
        }

        public DelegateCommand VideoClick { get; set; }

        public DelegateCommand<System.Windows.Input.KeyEventArgs> VideoEnter { get; set; }


        public LibraryViewModel(ISyncService syncService, IRegionManager regionManager, IUserConfigHelper configHelper, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _syncService = syncService;
            _eventAggregator = eventAggregator;
            VideoClick = new DelegateCommand(VideoClickExecute);
            VideoEnter = new DelegateCommand<System.Windows.Input.KeyEventArgs>(VideoEnterExecute);
            _configHelper = configHelper;
            _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = false});
        }

        private ObservableCollection<VideoFile> _images;

        public ObservableCollection<VideoFile> Images
        {
            get => _images;
            set => _ = SetProperty(ref _images, value);
        }

        private string _hi;

        public string Hi
        {
            get => _hi;
            set => _ = SetProperty(ref _hi, value);
        }

        private bool _loading = true;

        public bool Loading
        {
            get => _loading;
            set => _ = SetProperty(ref _loading, value);
        }


        private DelegateCommand _commandLoad = null;

        public DelegateCommand CommandLoad => _commandLoad ??= new DelegateCommand(CommandLoadExecute);

        private async Task<List<VideoFile>> GetAllClips(string filter)
        {
            var currentDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            await _syncService.ReIndexClips(); // maybe add a timer which blocks navigating on a setinterval type thing
            List<VideoFile>
                clips = await _syncService.GetAllUserClips(filter); // add a check to see if the clips have a thumbnail
            await _syncService.GenerateAllThumbnails(); // kind of bad code
            foreach (var clip in clips) // maybe using a cancellation token on the async methods can help me fix the bug
            {
                if (!File.Exists(clip.ThumbnailLocation))
                {
                    clip.ThumbnailLocation =
                        Path.Join(currentDir, "placeholder.jpg"); // add a right click context menu which sais "open file location" to file
                }
            }

            return clips;
        }

        private async void CommandLoadExecute()
        {
            List<VideoFile> clips = await GetAllClips("");
            /*
            Unosquare.FFME.Library.FFmpegDirectory = @"C:\Users\Nevin\Desktop\resync\bin\ffmpeg\x64\bin"; // make sure to add a check for 32 bit or 64 bit, and set the library according to that
            bool done = Unosquare.FFME.Library.LoadFFmpeg();
            if (!done)
            {
                MessageBox.Show("Error Loading FFMPEG");
            }
            */
            Images = new ObservableCollection<VideoFile>(clips);
            Loading = false;
            var allGames = await _syncService.GetAllGames();
            Hi = "new epic";
        }

        public bool IsNavigationTarget(NavigationContext navigationContext)
        {
            return true; // no idea what this does yet
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
            getVideosTask ??= Task.Run(() => CommandLoadExecute()); // make sure to program a "refresh videos" button somewhere
        }

        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void ConfirmNavigationRequest(NavigationContext navigationContext, Action<bool> continuationCallback)
        {
            bool result;
            if (getVideosTask.Status.Equals(TaskStatus.Running))
            {
                result = false;
            }
            else result = true;

            continuationCallback(result);
        }

        private void VideoClickExecute()
        {
            HandleVideoSelect();
        }

        public void VideoEnterExecute(System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                HandleVideoSelect();
            }
        }

        private void HandleVideoSelect()
        {
            if (SelectedFile != null)
            {
                var navigationParameters = new NavigationParameters();
                navigationParameters.Add("UserVideos", SelectedFile.VideoLocation);
                _regionManager.RequestNavigate("ContentRegion", new Uri("VideoPlayer" + navigationParameters.ToString(), UriKind.Relative));
                _eventAggregator.GetEvent<MenuBarEvent>().Publish(new MenuBarEventArgs() {Open = true});
                // _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
        }

    }
}
