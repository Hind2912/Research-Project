using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MusicBoxProj.Models;
using System.Drawing;

namespace MusicBoxProj.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        public DbSet<MusicBoxProj.Models.Album> Albums { get; set; } = default!;
        public DbSet<MusicBoxProj.Models.Band> Bands { get; set; }

        public DbSet<MusicBoxProj.Models.Genre> Genres { get; set; }

        public DbSet<MusicBoxProj.Models.Song> Songs { get; set; }
       

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


           

           





        }

        public DbSet<MusicBoxProj.Models.PlayList> PlayList { get; set; }


    }
}