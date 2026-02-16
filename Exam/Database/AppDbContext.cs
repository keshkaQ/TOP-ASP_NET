using Exam.Models;
using Microsoft.EntityFrameworkCore;

namespace Exam.Database
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Client> Clients { get; set; }
        public DbSet<TennisCourt> Courts {  get; set; }
        public DbSet<Booking> Bookings { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Client>(client =>
            {
                client.HasIndex(c => c.Email).IsUnique();
                client.HasMany(c => c.Bookings)
                .WithOne(b => b.Client)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            });
                

            modelBuilder.Entity<TennisCourt>(tc =>
            {
                tc.HasIndex(c => c.Name).IsUnique();
                tc.HasMany(t => t.Bookings)
                .WithOne(b => b.TennisCourt)
                .HasForeignKey(b => b.TennisCourtId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Booking>(b =>
            {
                b.HasOne(b =>b.TennisCourt)
                .WithMany(tc => tc.Bookings)
                .HasForeignKey(b => b.TennisCourtId)
                .OnDelete(DeleteBehavior.Cascade);

                b.HasOne(b => b.Client)
                .WithMany(c => c.Bookings)
                .HasForeignKey(b => b.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            });
           
        }
    }
}
