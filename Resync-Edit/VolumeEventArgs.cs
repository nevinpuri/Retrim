using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resync_Edit
{
    class VolumeEventArgs : EventArgs
    {
        public VolumeEventArgs(double volume)
        {
            Volume = volume;
        }

        public double Volume { get; }
    }
}
