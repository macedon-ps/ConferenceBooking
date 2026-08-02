using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

public class BookingConfiguration
    : IEntityTypeConfiguration<Booking>
{
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

        builder.HasMany<BookingService>()
            .WithOne()
            .HasForeignKey(x => x.BookingId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(nameof(Booking.Services))
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}