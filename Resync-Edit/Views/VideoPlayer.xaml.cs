using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Resync_Edit.ViewModels;

namespace Resync_Edit.Views
{
    /// <summary>
    /// Interaction logic for VideoPlayer.xaml
    /// </summary>
    public partial class VideoPlayer : UserControl
    {
        VideoPlayerViewModel vm = new VideoPlayerViewModel();
        public VideoPlayer()
        {
            InitializeComponent();
            this.DataContext = vm;
        }
    }
}
