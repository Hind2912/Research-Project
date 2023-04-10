using Microsoft.AspNetCore.Mvc.Rendering;
using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class PlayListCreateVM
    {
        public int PlayListId { get; set; }
        public string PlayListName { get; set; } = string.Empty;
        //public List<PlayListSong>? ListOfSongs { get; set; }
        //public string SongFilePath { get; set; } = string.Empty;

        public int[]? SelectedSongIDs { get; set; }
        public MultiSelectList? SongSelectList { get; set; }
    }
}
