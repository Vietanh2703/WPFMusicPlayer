using Microsoft.Win32.SafeHandles;
using MusicPlayer.DAL.Entities;
using MusicPlayer.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.BLL.Services
{
    public class PlaylistService
    {
        private PlaylistRepository _repo = new();
        public void AddSong(Playlist song)
        {
            _repo.Add(song);

        }
        public List<Playlist> GetAllPlaylistByUser(int userId) 
        { 
            return _repo.GetAll(userId);
        }



        public void UpdateSong(Playlist song, int playlistId)
        {
            _repo.Update(song, playlistId);

        }

        public void DeleteSong(Playlist song, int playlistId)
        {
            _repo.Delete(song, playlistId);
        }
    }
}
