using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore.Design;

namespace SyncServiceLibrary
{

    public class ClipContext : DbContext
    {
        public DbSet<VideoFile> VideoFiles { get; set; }

        public ClipContext(DbContextOptions<ClipContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={UserConfigHelper.GetDbPath()}");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
