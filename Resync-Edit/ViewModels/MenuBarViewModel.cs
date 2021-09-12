using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using ABI.Windows.Devices.Bluetooth.Background;
using FFMpegCore;
using Microsoft.Toolkit.Uwp.Notifications;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using Prism.Regions;
using Prism.Services.Dialogs;
using Resync_Edit.Events;

namespace Resync_Edit.ViewModels
{
    public class MenuBarViewModel : BindableBase
    {

        private string _currentlyLoadedVideo;

        private string _currentVideoName;

        private double _minThumb = Double.NaN;

        private double _maxThumb =  Double.NaN;

        private IRegionManager _regionManager;

        private IEventAggregator _eventAggregator;

        public string CurrentlyLoadedVideo
        {
            get => _currentlyLoadedVideo;
            set => SetProperty(ref _currentlyLoadedVideo, value);
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

        private DelegateCommand _previousNavigate;
        public DelegateCommand PreviousNavigate => _previousNavigate ??= new DelegateCommand(PreviousNavigate_Execute);

        private DelegateCommand _saveCopyCommand;

        public DelegateCommand SaveCopyCommand => _saveCopyCommand ??= new DelegateCommand(SaveCopy_Execute);

        private async void SaveCopy_Execute()
        {
            if (CurrentlyLoadedVideo is null || Double.IsNaN(MinThumb) || Double.IsNaN(MaxThumb)) return;
            SaveFileDialog fileSave = new SaveFileDialog
            {
                Title = "Select Location to Save Copy",
                FileName = $"TRIM - {Path.GetFileName(_currentVideoName)}"
            };
            if (fileSave.ShowDialog() == false)
            {
                return;
            }

            if (fileSave.FileName == "")
            {
                return;
            }

            _eventAggregator.GetEvent<VideoExportingEvent>().Publish(true);
            await FFMpegArguments.FromFileInput(CurrentlyLoadedVideo, true, options => options
                    .UsingMultithreading(true)
                    .Seek(TimeSpan.FromSeconds(MinThumb))
                    .WithDuration(TimeSpan.FromSeconds(MaxThumb)))
                .OutputToFile(fileSave.FileName, true, options => options
                    .WithCustomArgument("-c copy")
                    .WithFastStart())
                .ProcessAsynchronously();
            _eventAggregator.GetEvent<VideoExportingEvent>().Publish(false);
            new ToastContentBuilder().AddText("Your video has finished exporting!").Show();
            MessageBox.Show("done");
        }

        public MenuBarViewModel(IRegionManager regionManager, IEventAggregator eventAggregator)
        {
            _regionManager = regionManager;
            _eventAggregator = eventAggregator;
            eventAggregator.GetEvent<VideoPlayerEvent>().Subscribe(s =>
            {
                CurrentlyLoadedVideo = s;
                _currentVideoName = Path.GetFileName(CurrentlyLoadedVideo);
            });
            eventAggregator.GetEvent<ThumbChangeEvent>().Subscribe(args =>
            {
                MinThumb = args.MinThumb;
                MaxThumb = args.MaxThumb;
            });
        }

        private void PreviousNavigate_Execute()
        {
            _regionManager.RequestNavigate("ContentRegion", "MainMenu");
        //_regionManager.Deactivate("MenuBar");
        }
    }
}
