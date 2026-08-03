using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

/// <summary>
/// Клас конфігурації для сутності <see cref="BookingService"/>.
/// </summary>
public class BookingServiceConfiguration
    : IEntityTypeConfiguration<BookingService>
{
    /// <summary>
    /// Метод для налаштування конфігурації сутності <see cref="BookingService"/> за допомогою <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder">Будівельник сутності</param>
    public void Configure(EntityTypeBuilder<BookingService> builder)
    {
        builder.ToTable("BookingServices");

        builder.HasKey(x => new
        {
            x.BookingId,
            x.ServiceId
        });

        builder.HasOne<Booking>()
           .WithMany(x => x.Services)
           .HasForeignKey(x => x.BookingId)
           .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Service>()
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}