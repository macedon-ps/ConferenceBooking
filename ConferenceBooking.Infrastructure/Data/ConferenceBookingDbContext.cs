using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data
{
    public class ConferenceBookingDbContext: DbContext
    {
        public ConferenceBookingDbContext(
        DbContextOptions<ConferenceBookingDbContext> options)
        : base(options)
        {
        }

        public DbSet<Hall> Halls => Set<Hall>();
        public DbSet<Service> Services => Set<Service>();
        public DbSet<Booking> Bookings => Set<Booking>();
        public DbSet<HallService> HallServices => Set<HallService>();
        public DbSet<BookingService> BookingServices => Set<BookingService>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ConferenceBookingDbContext).Assembly);
        }
    }
}
