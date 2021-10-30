using System;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using Microsoft.WindowsAPICodePack.Shell.Interop;

namespace SyncServiceLibrary
{

    public class VideoFile
    {

        [Key]
        public int Id { get; set; }

        public string Title { get; set; }

        public string Game { get; set; }

        public string Date { get; set; }

        public bool Sync { get; set; }

        public string LocalLocation { get; set; }

        public string ThumbnailUrl { get; set; }

        public string ThumbnailLocation { get; set; } // might be a good idea to make two separate models, one for local and one for online

        public string OnlineLocation { get; set; }

        public long FileSize { get; set; } // mb

        public string VideoLocation { get; set; }

        public string VideoUri { get; set; }

        public VideoFile(string title, string game, long fileSize, string videoLocation, string thumbnailLocation)
        {
            Title = title;
            Game = game;
            FileSize = fileSize;
            Date = "5/30/2021";
            Sync = false;
            VideoLocation = videoLocation;
            ThumbnailLocation = thumbnailLocation;
        }
    }
}
