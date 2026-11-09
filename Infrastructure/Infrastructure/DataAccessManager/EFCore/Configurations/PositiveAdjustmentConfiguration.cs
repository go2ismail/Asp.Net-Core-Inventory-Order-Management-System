using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class PositiveAdjustmentConfiguration : BaseEntityConfiguration<PositiveAdjustment>
{
    public override void Configure(EntityTypeBuilder<PositiveAdjustment> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Number).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.AdjustmentDate).IsRequired(false);
        builder.Property(x => x.Status).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.Number);
    }
}

