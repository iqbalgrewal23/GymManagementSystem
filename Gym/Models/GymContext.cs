using Microsoft.EntityFrameworkCore;

namespace Gym.Models
{
    public class GymContext : DbContext
    {
        public GymContext(DbContextOptions<GymContext> options) : base(options) {}

        public DbSet<Member> Members { get; set; }
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<GymClass> GymClasses { get; set; }
        public DbSet<MemberClass> MemberClasses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MemberClass>()
                .HasKey(mc => new { mc.MemberId, mc.GymClassId });

            modelBuilder.Entity<MemberClass>()
                .HasOne(mc => mc.Member)
                .WithMany(m => m.MemberClasses)
                .HasForeignKey(mc => mc.MemberId);

            modelBuilder.Entity<MemberClass>()
                .HasOne(mc => mc.GymClass)
                .WithMany(gc => gc.MemberClasses)
                .HasForeignKey(mc => mc.GymClassId);
        }
    }
}
