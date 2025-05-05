using Microsoft.EntityFrameworkCore;
using Pixeval.Backend.Models;

namespace Pixeval.Backend;

public class PixevalDbContext(DbContextOptions<PixevalDbContext> ctx) : DbContext(ctx)
{
    public DbSet<Illustration> Illustrations { get; set; }
    
    public DbSet<User> Users { get; set; }
    
    public DbSet<FavoriteItem> FavoriteList { get; set; }
    
    public DbSet<FollowItem> FollowList { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.EnableSensitiveDataLogging();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Illustration>()
            .HasOne(e => e.User)
            .WithMany(t => t.Posts)
            .HasForeignKey(e => e.UserId);

        modelBuilder.Entity<FavoriteItem>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<FavoriteItem>()
            .HasOne(f => f.Illustration)
            .WithMany()
            .HasForeignKey(f => f.IllustrationId);

        modelBuilder.Entity<FollowItem>()
            .HasOne(f => f.User)
            .WithMany()
            .HasForeignKey(f => f.UserId);

        modelBuilder.Entity<FollowItem>()
            .HasOne(f => f.FollowedUser)
            .WithMany()
            .HasForeignKey(f => f.FollowedUserId);
    }
}
