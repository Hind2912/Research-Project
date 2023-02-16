using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class AlbumCreateVM
    {
        public string AlbumTitle { get; set; } = string.Empty;
        public int[]? GenresIds { get; set; }
        public int BandId { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = new DateTime();

        public SelectList? BandSelectList { get; set; }

        public MultiSelectList? GenreSelectList { get; set; }

        public MultiSelectList? SongSelectList { get; set; }
    }
}
