using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class BillOfMaterialConfiguration : BaseEntityConfiguration<BillOfMaterial>
{
    public override void Configure(EntityTypeBuilder<BillOfMaterial> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.ProductId).HasMaxLength(IdConsts.MaxLength).IsRequired();
        builder.Property(x => x.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Version).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.IsActive).IsRequired().HasDefaultValue(true);
        builder.Property(x => x.EffectiveFrom).IsRequired(false);
        builder.Property(x => x.EffectiveTo).IsRequired(false);

        builder.HasIndex(e => e.ProductId);
        builder.HasIndex(e => e.IsActive);
        builder.HasIndex(e => new { e.ProductId, e.Version, e.IsActive });

        builder.HasOne(x => x.Product)
            .WithMany(p => p.BillOfMaterials)
            .HasForeignKey(x => x.ProductId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
