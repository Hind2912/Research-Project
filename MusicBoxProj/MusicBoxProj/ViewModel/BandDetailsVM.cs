using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class BandDetailsVM
    {
        public int BandId { get; set; }
        public string BandName { get; set; } = string.Empty;
        public List<Album>? Albums { get; set; }
        public List<Song>? Songs { get; set; }

        public BandDetailsVM()
        {
            Songs = new List<Song>();
            Albums = new List<Album>();
        }
    }
}
