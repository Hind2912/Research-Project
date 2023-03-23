using Microsoft.AspNetCore.Mvc.Rendering;
using MusicBoxProj.Models;
using System.ComponentModel.DataAnnotations;

namespace MusicBoxProj.ViewModel
{
    public class SongEditVM
    {
        public int SongId { get; set; }
        
        public string SongName { get; set; } = string.Empty;

        public int AlbumId { get; set; }
        public Album? Album { get; set; }
        public SelectList? AlbumSelectList { get; set; }
        public string AlbumName { get; set; } = string.Empty;
        public int? BandId { get; set; }
        public Band? Band { get; set; }
        public SelectList? BandSelectList { get; set; }
        public string BandName { get; set; } = string.Empty;
        public string? SongDuration { get; set; }

        public string SongFilePath { get; set; } = string.Empty;
        public int[]? PlayListIDs { get; set; }
        public List<PlayListSong>? ListOfPlayLists { get; set; }
    }
}
