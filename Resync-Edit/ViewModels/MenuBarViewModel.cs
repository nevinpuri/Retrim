using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Regions;

namespace Resync_Edit.ViewModels
{
    public class MenuBarViewModel : BindableBase
    {
        private IRegionManager _regionManager;

        private DelegateCommand _previousNavigate;
        public DelegateCommand PreviousNavigate => _previousNavigate ??= new DelegateCommand(PreviousNavigate_Execute);

        public MenuBarViewModel(IRegionManager regionManager)
        {
            _regionManager = regionManager;
        }

        private async void PreviousNavigate_Execute()
        {
            _regionManager.RequestNavigate("ContentRegion", "MainMenu");
            //_regionManager.Deactivate("MenuBar");
        }
    }
}
