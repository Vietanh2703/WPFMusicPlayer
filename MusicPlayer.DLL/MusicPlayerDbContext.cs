using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using MusicPlayer.DAL.Entities;

namespace MusicPlayer.DAL;

public partial class MusicPlayerDbContext : DbContext
{
    public MusicPlayerDbContext()
    {
    }

    public MusicPlayerDbContext(DbContextOptions<MusicPlayerDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<UserAccount> UserAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Server=(local);uid=sa;pwd=1;database= MusicPlayerDb;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<UserAccount>(entity =>
        {
            entity.HasKey(e => e.MemberId);

            entity.ToTable("UserAccount");

            entity.Property(e => e.MemberId).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(50);
            entity.Property(e => e.FullName).HasMaxLength(50);
            entity.Property(e => e.Password).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
