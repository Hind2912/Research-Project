using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class GenreDetailsVM
    {
        public int GenreId { get; set; }
        public string GenreName { get; set; } = string.Empty;

        public string AlbumName { get; set; }

        public List<Album>? Albums { get; set; }


        public GenreDetailsVM()
        {
            Albums = new List<Album>();

        }
    }
}
