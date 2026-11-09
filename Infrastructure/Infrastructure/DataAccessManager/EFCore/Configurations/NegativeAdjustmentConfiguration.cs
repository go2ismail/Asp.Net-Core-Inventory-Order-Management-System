using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class NegativeAdjustmentConfiguration : BaseEntityConfiguration<NegativeAdjustment>
{
    public override void Configure(EntityTypeBuilder<NegativeAdjustment> builder)
    {
        base.Configure(builder);

        builder.Property(c => c.Number).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(c => c.AdjustmentDate).IsRequired(false);
        builder.Property(c => c.Status).IsRequired(false);
        builder.Property(c => c.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.Number);
    }
}
