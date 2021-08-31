using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Microsoft.Win32;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = System.Windows.Forms.OpenFileDialog;

namespace Resync_Edit.ViewModels
{
    class MainMenuViewModel : BindableBase
    {

        private IRegionManager _regionManager;

        private DelegateCommand _fileSelect;

        public DelegateCommand FileSelect => _fileSelect ??= new DelegateCommand(FileSelect_Execute);

        public MainMenuViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private void FileSelect_Execute()
        {
            string filePath;
            using (OpenFileDialog fileDialog = new OpenFileDialog())
            {
                fileDialog.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyVideos);
                fileDialog.RestoreDirectory = true;
                if (fileDialog.ShowDialog() != DialogResult.OK) return;
                filePath = fileDialog.FileName;
                var navigationParameters = new NavigationParameters
                {
                    { "UserVideos", fileDialog.FileName }
                };
                _regionManager.RequestNavigate("ContentRegion", new Uri("VideoPlayer" + navigationParameters.ToString(), UriKind.Relative));
                _regionManager.RequestNavigate("MenuRegion", "MenuBar");
            }
        }

    }
}
