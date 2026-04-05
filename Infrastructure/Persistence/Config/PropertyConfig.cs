using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Config;

public class PropertyConfig : IEntityTypeConfiguration<Property>
{
    public void Configure(EntityTypeBuilder<Property> builder)
    {
        builder.HasKey(p => p.Id);

        builder.Property(p => p.Title).IsRequired().HasMaxLength(200);
        builder.Property(p => p.Description).IsRequired().HasMaxLength(2000);
        builder.Property(p => p.Location).IsRequired().HasMaxLength(500);
        
        builder.Property(p => p.PricePerNight).HasColumnType("decimal(18,2)");

        builder.Property(p => p.RowVersion)
               .IsRowVersion();

        builder.OwnsMany(p => p.BlockedDates, bd =>
        {
            bd.ToTable("PropertyBlockedDates");

            bd.Property(r => r.Start).HasColumnName("StartDate").IsRequired();
            bd.Property(r => r.End).HasColumnName("EndDate").IsRequired();
            
            bd.WithOwner().HasForeignKey("PropertyId");
            bd.HasKey("PropertyId", "Start", "End"); // Use CLR property names for the key
        });

        builder.Metadata.FindNavigation(nameof(Property.BlockedDates))!
               .SetPropertyAccessMode(PropertyAccessMode.Field);

        builder.Property(p => p.ImageUrls)
               .HasField("_imageUrls")
               .UsePropertyAccessMode(PropertyAccessMode.Field);

    }
}