using Microsoft.EntityFrameworkCore;
using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Group> Groups { get; set; } = null!;
        public DbSet<GroupMembership> GroupMemberships { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("secret_friend_db");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Username)
                      .IsRequired()
                      .HasMaxLength(100);
                entity.Property(u => u.Email)
                      .IsRequired()
                      .HasMaxLength(200);
                entity.Property(u => u.KeycloakId)
                     .IsRequired();
                entity.Property(u => u.CreatedAt)
                      .IsRequired();
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.HasKey(g => g.Id);

                entity.Property(g => g.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(g => g.Description)
                      .HasMaxLength(500);

                entity.Property(g => g.CreatedAt)
                      .IsRequired();

                entity.Property(g => g.IsDrawn)
                      .IsRequired();

                entity.HasOne(g => g.CreatedBy)
                      .WithMany(u => u.Groups)
                      .HasForeignKey(g => g.CreatedById)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<GroupMembership>(entity =>
            {
                entity.HasKey(m => m.Id);

                entity.Property(m => m.JoinedAt)
                      .IsRequired();

                entity.Property(m => m.IsAdmin)
                      .IsRequired();

                entity.HasOne(m => m.Group)
                      .WithMany(g => g.Members)
                      .HasForeignKey(m => m.GroupId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.User)
                      .WithMany(u => u.Memberships)
                      .HasForeignKey(m => m.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(gm => gm.AssignedFriend)
       .WithMany()
       .HasForeignKey(gm => gm.AssignedFriendId)
       .OnDelete(DeleteBehavior.Restrict);

                entity.Property(gm => gm.GiftRecommendation)
                       .HasMaxLength(500);

            });
        }
    }
}
