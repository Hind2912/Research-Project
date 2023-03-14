using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class AlbumCreateVM
    {
        public string AlbumName { get; set; } = string.Empty;
        public int[]? GenresIds { get; set; }
        public string GenreName { get; set; } = string.Empty;   
        public int BandId { get; set; }
        public string BandName { get; set; }=string.Empty;  

        public string SongName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Please enter a Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime ReleaseDate { get; set; }

        public SelectList? BandSelectList { get; set; }

        public MultiSelectList? GenreSelectList { get; set; }

        public MultiSelectList? SongSelectList { get; set; }
    }
}
