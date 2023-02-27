using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class AlbumCreateVM
    {
        public string AlbumName { get; set; } = string.Empty;
        public int[]? GenresIds { get; set; }
        public int BandId { get; set; }

        public string SongName { get; set; } = string.Empty;

        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }

        public SelectList? BandSelectList { get; set; }

        public MultiSelectList? GenreSelectList { get; set; }

        public MultiSelectList? SongSelectList { get; set; }
    }
}
