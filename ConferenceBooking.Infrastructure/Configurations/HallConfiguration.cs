using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

/// <summary>
/// Клас конфігурації для сутності <see cref="Hall"/>.
/// </summary>
public class HallConfiguration : IEntityTypeConfiguration<Hall>
{
    /// <summary>
    /// Метод для налаштування сутності <see cref="Hall"/> за допомогою <see cref="EntityTypeBuilder{TEntity}"/>.
    /// </summary>
    /// <param name="builder">Будівельник сутності</param>
    public void Configure(EntityTypeBuilder<Hall> builder)
    {
        builder.ToTable("Halls");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Capacity)
            .IsRequired();

        builder.Property(x => x.HourlyRate)
            .IsRequired()
            .HasPrecision(18, 2);

        builder.Navigation(x => x.Services)
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}