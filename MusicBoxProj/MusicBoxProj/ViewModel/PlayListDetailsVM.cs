﻿using Microsoft.AspNetCore.Mvc.Rendering;
using MusicBoxProj.Models;

namespace MusicBoxProj.ViewModel
{
    public class PlayListDetailsVM
    {
        public int PlayListId { get; set; }
        
        public string PlayListName { get; set; } = string.Empty;
        public List<PlayListSong>? ListOfSongs { get; set; }

        public int SongId { get; set; }
        
        public string SongName { get; set; } = string.Empty;

        public SelectList? SelectSongPath { get; set; }
        public string SongFilePath { get; set; } = string.Empty;

        public List<Song> Songs { get; set; }
       

        public PlayListDetailsVM()
        {
            Songs = new List<Song>();
        }
    }
}
