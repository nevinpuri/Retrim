using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resync_Edit
{
    class SeekEventArgs
    {
        public SeekEventArgs(double position, bool _isPlaying)
        {
            Position = position;
            isPlaying = _isPlaying;
        }

        public double Position { get; }

        public bool isPlaying { get; }
    }
}
