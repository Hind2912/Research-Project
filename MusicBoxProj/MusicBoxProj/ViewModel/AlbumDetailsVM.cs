using MusicBoxProj.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class AlbumDetailsVM
    {
        public int AlbumId { get; set; }
        public string AlbumTitle { get; set; } = string.Empty;
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; } = new DateTime();

        public Band? Band { get; set; }
        public List<Genre>? Genres { get; set; }
        public List<Song> Songs { get; set; }

        public AlbumDetailsVM() { 
            Genres = new List<Genre>();
            Songs= new List<Song>();
        }
    }
}
