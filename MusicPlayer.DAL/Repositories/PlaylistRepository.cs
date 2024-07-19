using MusicPlayer.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.DAL.Repositories
{
    public class PlaylistRepository
    {
        private MusicPlayerDbContext _context;
        public void Add(Playlist song)
        {
            _context = new();
            _context.Add(song);
            _context.SaveChanges();
        }

        public List<Playlist> GetAll(int userId)
        {
            _context = new();
            return _context.Playlists.Where(p => p.UserId == userId).ToList();
        }

        public void Update(Playlist song, int playlistId)
        {
            _context = new();
            var existingSong = Find(playlistId);
            if (existingSong != null)
            {
                existingSong.SongName = song.SongName; 
                existingSong.Url = song.Url;
                _context.Playlists.Update(existingSong);
                _context.SaveChanges();
            } 
        }
        public Playlist Find(int playlistId)
        {
            _context = new();
            return _context.Playlists.Find(playlistId);
        }
    }
}
