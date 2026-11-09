using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class CustomerConfiguration : BaseEntityConfiguration<Customer>
{
    public override void Configure(EntityTypeBuilder<Customer> builder)
    {
        base.Configure(builder);

        builder.Property(x => x.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Number).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Street).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.City).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.State).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.ZipCode).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Country).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.PhoneNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.FaxNumber).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.EmailAddress).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Website).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.WhatsApp).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.LinkedIn).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Facebook).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Instagram).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.TwitterX).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.TikTok).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.CustomerGroupId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.CustomerCategoryId).HasMaxLength(IdConsts.MaxLength).IsRequired(false);

        builder.HasIndex(e => e.Name);
        builder.HasIndex(e => e.Number);
    }
}

