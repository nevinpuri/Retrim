using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SyncServiceLibrary.Models
{
    public class UserConfig
    {
        public string FolderLocation { get; set; }

        public string ThumbnailLocation { get; set; }

        public List<string> WhitelistedFileTypes { get; set; }
    }
}
