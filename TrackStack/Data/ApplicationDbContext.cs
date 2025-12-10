using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TrackStack.Models;

namespace TrackStack.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // Optional: this DbSet<User> name collides with Identity's internal Users set.
        // It’s fine for now, but consider renaming later to avoid CS0114 warnings.
        public DbSet<User> Users { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Expenses> Expenses { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // SQLite compatibility for Identity
            builder.Entity<IdentityUser>(b => { b.Property(u => u.Id).HasColumnType("TEXT"); });
            builder.Entity<IdentityRole>(b =>
            {
                b.Property(r => r.Id).HasColumnType("TEXT");
                b.Property(r => r.ConcurrencyStamp).HasColumnType("TEXT");
            });
            builder.Entity<IdentityUserLogin<string>>(b => { b.Property(l => l.ProviderKey).HasColumnType("TEXT"); });
            builder.Entity<IdentityUserToken<string>>(b => { b.Property(t => t.Value).HasColumnType("TEXT"); });

            // Explicitly map Expenses -> IdentityUser via UserId (string)
            builder.Entity<Expenses>(b =>
            {
                b.HasOne<IdentityUser>(/* navigation: */ e => e.User)
                 .WithMany()
                 .HasForeignKey(e => e.UserId)
                 .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}