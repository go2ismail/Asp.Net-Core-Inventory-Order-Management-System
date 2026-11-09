using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class TokenConfiguration : BaseEntityConfiguration<Token>
{
    public override void Configure(EntityTypeBuilder<Token> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.UserId).HasMaxLength(UserIdConsts.MaxLength).IsRequired(false);
        builder.Property(e => e.RefreshToken).HasMaxLength(LengthConsts.M).IsRequired(false);
    }
}
