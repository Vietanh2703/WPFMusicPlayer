﻿using MusicPlayer.DAL.Entities;
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

        public void Update(Playlist song)
        {
            _context = new();
            _context.Playlists.Update(song);
            _context.SaveChanges();
        }
    }
}
