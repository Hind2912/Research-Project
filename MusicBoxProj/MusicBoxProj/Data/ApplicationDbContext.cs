using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicBoxProj.Models;
using System.Drawing;

namespace MusicBoxProj.Data
{

    public class ApplicationDbContext : IdentityDbContext
    {

        public DbSet<UserActivity> UserActivities { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<MusicBoxProj.Models.Album> Albums { get; set; } = default!;
        public DbSet<MusicBoxProj.Models.Band> Bands { get; set; }

        public DbSet<MusicBoxProj.Models.Genre> Genres { get; set; }

        public DbSet<MusicBoxProj.Models.Song> Songs { get; set; }
        public DbSet<MusicBoxProj.Models.PlayList> playLists  { get; set; }

        public DbSet<AlbumGenre> AlbumGenre { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<AlbumGenre>()
                .HasKey(ag => new { ag.AlbumId, ag.GenreId });
            modelBuilder.Entity<AlbumGenre>()
                 .HasOne(ag => ag.Album)
                .WithMany(a => a.ListOfGenres)
                .HasForeignKey(ag => ag.AlbumId);
            modelBuilder.Entity<AlbumGenre>()
                .HasOne(ag => ag.Genre)
                .WithMany(g => g.ListOfAlbums)
                .HasForeignKey(ag => ag.GenreId);

           
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<PlayListSong>()
                .HasKey(ps => new { ps.SongId, ps.PlayListId });
            modelBuilder.Entity<PlayListSong>()
                 .HasOne(ps => ps.Song)
                .WithMany(s => s.ListOfPlayLists)
                .HasForeignKey(ps => ps.SongId);
            modelBuilder.Entity<PlayListSong>()
                .HasOne(ps => ps.PlayList)
                .WithMany(p => p.ListOfSongs)
                .HasForeignKey(ps => ps.PlayListId);












        }

        public DbSet<MusicBoxProj.Models.PlayList> PlayList { get; set; }


    }
}