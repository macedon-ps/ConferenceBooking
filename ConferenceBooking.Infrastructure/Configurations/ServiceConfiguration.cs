using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

/// <summary>
/// Клас конфігурації для сутності <see cref="Service"/>.
/// </summary>
public class ServiceConfiguration : IEntityTypeConfiguration<Service>
{
    /// <summary>
    /// Метод для налаштування сутності <see cref="Service"/> у контексті бази даних.
    /// </summary>
    /// <param name="builder">Будівельник сутності</param>
    public void Configure(EntityTypeBuilder<Service> builder)
    {
        builder.ToTable("Services");

        builder.HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .ValueGeneratedNever();

        builder.Property(x => x.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(x => x.Price)
            .IsRequired()
            .HasPrecision(18, 2);
    }
}
