using Microsoft.EntityFrameworkCore;
using SANDBOX.Models;

namespace SANDBOX.Data
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();
        public DbSet<Problem> Problems => Set<Problem>();
        public DbSet<Tag> Tags => Set<Tag>();
        public DbSet<Group> Groups => Set<Group>();
        public DbSet<Homework> Homeworks => Set<Homework>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /// USERS >-----< PROBLEMS
            modelBuilder.Entity<User>()
                .HasMany(u => u.SolvedProblems)
                .WithMany(p => p.UsersWhoSolved);

            /// PROBLEMS >-----< TAGS
            modelBuilder.Entity<Problem>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Problems);

            /// GROUPS >-----< USERS
            modelBuilder.Entity<Group>()
                .HasMany(g => g.Users)
                .WithMany(u => u.Groups);

            /// GROUPS ------< HOMEWORKS
            modelBuilder.Entity<Homework>()
                .HasOne(h => h.group)
                .WithMany(g => g.HWs)
                .HasForeignKey(h => h.GroupId);

            /// PROBLEMS >-----< HOMEWORKS
            modelBuilder.Entity<Homework>()
                .HasMany(h => h.Problems)
                .WithMany(p => p.HWs);

            /// Unique TAG name
            modelBuilder.Entity<Tag>()
                .HasIndex(t => t.Name)
                .IsUnique();

            /// Unique USER name
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            /// Unique PROBLEM name
            modelBuilder.Entity<Problem>()
                .HasIndex(p => p.Name)
                .IsUnique();

            /// Unique GROUP name
            modelBuilder.Entity<Group>()
                .HasIndex(g => g.Name)
                .IsUnique();
        }
    }
}
