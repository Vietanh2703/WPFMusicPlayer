using System;
using System.Collections.Generic;

namespace MusicPlayer.DAL.Entities;

public partial class Playlist
{
    public int PlaylistId { get; set; }

    public int? UserId { get; set; }

    public string SongName { get; set; } = null!;

    public string Url { get; set; } = null!;

    public virtual UserAccount? User { get; set; }
}
