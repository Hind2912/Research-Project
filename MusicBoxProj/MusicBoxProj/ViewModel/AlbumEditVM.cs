using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class AlbumEditVM
    {
        public int AlbumId { get; set; }    
        public int BandId { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public SelectList? BandSelectList { get; set; }

        public int[]? GenreIds { get; set; }

        public MultiSelectList? GenreSelectList { get; set; }

        public int[]? SongIds { get; set; }

        public string SongName { get; set; } = string.Empty;

        public MultiSelectList? SongSelectList { get; set; }
    }
}
