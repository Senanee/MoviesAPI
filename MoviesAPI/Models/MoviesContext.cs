using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MoviesAPI.Models;

public partial class MoviesContext : DbContext
{
    public MoviesContext()
    {
    }

    public MoviesContext(DbContextOptions<MoviesContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailableGenre> AvailableGenres { get; set; }

    public virtual DbSet<Movie> Movies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailableGenre>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("AvailableGenre");

            entity.Property(e => e.Genre)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("genre");
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.Property(e => e.Genre)
                .HasMaxLength(256)
                .IsUnicode(false);
            entity.Property(e => e.OriginalLanguage)
                .HasMaxLength(2)
                .IsUnicode(false)
                .HasColumnName("Original_Language");
            entity.Property(e => e.Overview).IsUnicode(false);
            entity.Property(e => e.Popularity).HasColumnType("decimal(15, 3)");
            entity.Property(e => e.PosterUrl)
                .HasMaxLength(256)
                .IsUnicode(false)
                .HasColumnName("Poster_Url");
            entity.Property(e => e.ReleaseDate)
                .HasColumnType("datetime")
                .HasColumnName("Release_Date");
            entity.Property(e => e.Title)
                .HasMaxLength(512)
                .IsUnicode(false);
            entity.Property(e => e.VoteAverage)
                .HasColumnType("decimal(16, 2)")
                .HasColumnName("Vote_Average");
            entity.Property(e => e.VoteCount).HasColumnName("Vote_Count");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
