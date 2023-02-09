using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.Models
{
    public class Album
    {
        public int AlbumId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public int BandId { get; set; }
        public Band? Band { get; set; }
        public List<Song>? ListOfSongs { get; set; }

        public List<AlbumGenre>? ListOfGenres { get; set; }
    }


    public class Band
    {
        public int BandId { get; set; }
        [Required]
        public string BandName { get; set; } = string.Empty;
        public List<Album>? ListOfAlbums { get; set; }
        public List<Song>? ListOfSongs { get; set; }

    }

    public class Song
    {
        public int SongId { get; set; }
        [Required]
        public string SongName { get; set; } = string.Empty;

        public int AlbumId { get; set; }
        public Album? Album { get; set; }  
       
        public int PlayListId { get; set; }

        public PlayList? PlayList { get; set; }
        public int SongDuration { get; set; }   

        public string SongFilePath { get; set; } = string.Empty;
    }

    public class Genre
    {
        public int GenreId { get; set; }
        [Required]
        public string GenreName { get; set; } = string.Empty;

        public List<AlbumGenre>? ListOfAlbums { get; set; }
    }

    public class AlbumGenre
    {
        public int AlbumId { get; set; }

        public Album? Album { get; set; }

        public int GenreId { get; set; }

        public Genre? Genre { get; set; }

    }

    public class PlayList
    {
        public int PlayListId { get; set; }
        [Required]
        public string PlayListName { get; set; } = string.Empty;
        public List<Song>? ListOfSongs { get; set; }
    }
}
