using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Resync_Edit
{
    public interface IMediaService
    {
        void Play();
        void Pause();
        void Stop();
        void LoadMedia(Uri mediaUri);
        void SetVolume(double volume);
    }
}
