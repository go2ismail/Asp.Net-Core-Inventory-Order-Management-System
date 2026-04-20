using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class BillOfMaterialItemConfiguration : BaseEntityConfiguration<BillOfMaterialItem>
{
    public override void Configure(EntityTypeBuilder<BillOfMaterialItem> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.BillOfMaterialId).HasMaxLength(IdConsts.MaxLength).IsRequired();
        builder.Property(x => x.ComponentProductId).HasMaxLength(IdConsts.MaxLength).IsRequired();
        builder.Property(x => x.Quantity).IsRequired().HasDefaultValue(1);
        builder.Property(x => x.Sequence).IsRequired(false);
        builder.Property(x => x.UnitMeasureId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.ScrapPercentage).IsRequired(false).HasDefaultValue(0);
        builder.Property(x => x.Notes).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.BillOfMaterialId);
        builder.HasIndex(e => e.ComponentProductId);

        builder.HasOne(x => x.BillOfMaterial)
            .WithMany(b => b.Items)
            .HasForeignKey(x => x.BillOfMaterialId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(x => x.ComponentProduct)
            .WithMany()
            .HasForeignKey(x => x.ComponentProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(x => x.UnitMeasure)
            .WithMany()
            .HasForeignKey(x => x.UnitMeasureId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
