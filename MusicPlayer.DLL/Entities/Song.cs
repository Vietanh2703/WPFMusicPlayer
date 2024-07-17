using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicPlayer.DAL.Entities
{
    public class Song
    {
        public int SongId { get; set; }

        public string Title { get; set; }

        public string Artist { get; set; }

        public string Album { get; set; }

        public string Genre { get; set; }

        public string Path { get; set; }

        public int Year { get; set; }

        public int Duration { get; set; }

        public int PlayCount { get; set; }

        public int SkipCount { get; set; }

        public DateTime DateAdded { get; set; }

        public bool IsFavorite { get; set; }

        public bool IsDeleted { get; set; }

        public bool IsSynced { get; set; }

        public bool IsPlaying { get; set; }

        public bool IsSkipped { get; set; }

        public bool IsPlayed { get; set; }

        public bool IsPaused { get; set; }

        public bool IsStopped { get; set; }

        public bool IsMuted { get; set; }

        public bool IsShuffled { get; set; }

        public bool IsLooped { get; set; }

        public bool IsRepeatOne { get; set; }

        public bool IsRepeatAll { get; set; }

        public bool IsRepeatNone { get; set; }

        public bool IsRepeat { get; set; }

        public bool IsPlayingNow { get; set; }

        public bool IsPausedNow { get; set; }

        public bool IsStoppedNow { get; set; }

        public bool IsMutedNow { get; set; }

        public bool IsShuffledNow { get; set; }

        public bool IsLoopedNow { get; set; }

        public bool IsRepeatOneNow { get; set; }

        public bool IsRepeatAllNow { get; set; }

        public bool IsRepeatNoneNow { get; set; }

        public bool IsRepeatNow { get; set; }

        public bool IsPlayingNext { get; set; }

        public bool IsPausedNext { get; set; }

        public bool IsStoppedNext { get; set; }

        public bool IsMutedNext { get; set; }
    }
}
