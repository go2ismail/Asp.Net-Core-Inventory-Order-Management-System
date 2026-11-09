using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class CompanyConfiguration : BaseEntityConfiguration<Company>
{
    public override void Configure(EntityTypeBuilder<Company> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Currency).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Street).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.City).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.State).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.ZipCode).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Country).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.PhoneNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.FaxNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.EmailAddress).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Website).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
    }
}

