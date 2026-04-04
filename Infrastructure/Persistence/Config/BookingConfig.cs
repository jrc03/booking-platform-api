using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config;

public class BookingConfiguration : IEntityTypeConfiguration<Booking>
{
    public void Configure(EntityTypeBuilder<Booking> builder)
    {
        builder.HasKey(b => b.Id);

        builder.Property(b => b.Status)
               .HasConversion<string>()
               .IsRequired();
       
        builder.OwnsOne(b => b.Dates, d =>
        {
            d.Property(p => p.Start).HasColumnName("StartDate").IsRequired();
            d.Property(p => p.End).HasColumnName("EndDate").IsRequired();
        });
     
        builder.Property(b => b.RowVersion)
               .IsRowVersion();
    }
}