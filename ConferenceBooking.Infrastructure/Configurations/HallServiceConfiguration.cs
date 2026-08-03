using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

/// <summary>
/// Клас конфігурації для сутності <see cref="HallService"/>.
/// </summary>
public class HallServiceConfiguration
    : IEntityTypeConfiguration<HallService>
{
    /// <summary>
    /// Метод для налаштування сутності <see cref="HallService"/> у моделі даних.
    /// </summary>
    /// <param name="builder">Будівельник сутності</param>
    public void Configure(EntityTypeBuilder<HallService> builder)
    {
        builder.ToTable("HallServices");

        builder.HasKey(x => new
        {
            x.HallId,
            x.ServiceId
        });

        builder.HasOne<Hall>()
            .WithMany(x => x.Services)
            .HasForeignKey(x => x.HallId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne<Service>()
            .WithMany()
            .HasForeignKey(x => x.ServiceId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}