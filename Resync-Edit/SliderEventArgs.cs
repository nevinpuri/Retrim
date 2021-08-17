using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resync_Edit
{
    class SliderEventArgs : EventArgs
    {

        public SliderEventArgs(double position)
        {
            Position = position;
        }

        public double Position { get; }
    }
}
