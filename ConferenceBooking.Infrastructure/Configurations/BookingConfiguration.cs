using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

/// <summary>
/// Клас конфігурації для сутності <see cref="Booking"/>.
/// </summary>
public class BookingConfiguration
    : IEntityTypeConfiguration<Booking>
{
    /// <summary>
    /// Метод для налаштування конфігурації сутності <see cref="Booking"/> за допомогою <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder">Будівельник сутності</param>
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.ToTable("Bookings");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.HallId)
            .IsRequired();

        builder.Property(x => x.StartTime)
            .IsRequired();

        builder.Property(x => x.EndTime)
            .IsRequired();

        builder.Property(x => x.TotalCost)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.HasOne<Hall>()
            .WithMany()
            .HasForeignKey(x => x.HallId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Navigation(x => x.Services)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}