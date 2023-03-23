using Microsoft.AspNetCore.Mvc.Rendering;
using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class PlayListEditVM
    {
        public int PlayListId { get; set; }
        
        public string PlayListName { get; set; } = string.Empty;
        public int[]? SongIds { get; set; }

        public string SongName { get; set; } = string.Empty;

        public MultiSelectList? SongSelectList { get; set; }
    }
}
