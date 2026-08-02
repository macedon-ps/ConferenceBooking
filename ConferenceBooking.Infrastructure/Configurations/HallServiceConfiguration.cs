using ConferenceBooking.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ConferenceBooking.Infrastructure.Configurations;

public class HallServiceConfiguration
    : IEntityTypeConfiguration<HallService>
{
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