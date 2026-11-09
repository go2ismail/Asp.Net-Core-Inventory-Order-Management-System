using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class NumberSequenceConfiguration : BaseEntityConfiguration<NumberSequence>
{
    public override void Configure(EntityTypeBuilder<NumberSequence> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.EntityName).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Prefix).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Suffix).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.EntityName);
    }
}
