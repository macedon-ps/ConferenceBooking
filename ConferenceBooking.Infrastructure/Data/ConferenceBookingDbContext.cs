using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ConferenceBooking.Infrastructure.Data
{
    /// <summary>
    /// Клас ConferenceBookingDbContext представляє контекст бази даних для системи бронювання конференцій. Він містить DbSet-и для залів, послуг, бронювань та зв'язків між ними.
    /// </summary>
    public class ConferenceBookingDbContext: DbContext
    {
        /// <summary>
        /// Конструктор класу ConferenceBookingDbContext, який приймає параметри конфігурації контексту бази даних.
        /// </summary>
        /// <param name="options">Параметри конфігурації контексту бази даних</param>
        public ConferenceBookingDbContext(
        DbContextOptions<ConferenceBookingDbContext> options)
        : base(options)
        {
        }

        /// <summary>
        /// Колекція залів конференцій у базі даних.
        /// </summary>
        public DbSet<Hall> Halls => Set<Hall>();

        /// <summary>
        /// Колекція послуг у базі даних.
        /// </summary>
        public DbSet<Service> Services => Set<Service>();

        /// <summary>
        /// Колекція бронювань у базі даних.
        /// </summary>
        public DbSet<Booking> Bookings => Set<Booking>();

        /// <summary>
        /// Колекція зв'язків між залами та послугами у базі даних.
        /// </summary>
        public DbSet<HallService> HallServices => Set<HallService>();

        /// <summary>
        /// Колекція зв'язків між бронюваннями та послугами у базі даних.       
        /// </summary>
        public DbSet<BookingService> BookingServices => Set<BookingService>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(ConferenceBookingDbContext).Assembly);
        }
    }
}
