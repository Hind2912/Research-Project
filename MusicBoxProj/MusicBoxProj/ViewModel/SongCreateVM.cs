using Microsoft.AspNetCore.Mvc.Rendering;
using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class SongCreateVM
    {
        public int SongId { get; set; }
       
        public string SongName { get; set; } = string.Empty;

        public int AlbumId { get; set; }
        public Album? Album { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public SelectList? AlbumSelectList { get; set; }
        public int? BandId { get; set; }
        public Band? Band { get; set; }
        public string BandName { get; set; } = string.Empty;
        public SelectList? BandSelectList { get; set; }
        public string? SongDuration { get; set; }

        public string SongFilePath { get; set; } = string.Empty;

        public List<PlayListSong>? ListOfPlayLists { get; set; }
    }
}
