using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

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
            MessageBox.Show(Environment.GetCommandLineArgs()[0]);
        }

        private void FileSelect_Execute()
        {
        }

    }
}
