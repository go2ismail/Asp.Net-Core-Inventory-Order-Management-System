using Domain.Entities;
using Infrastructure.DataAccessManager.EFCore.Common;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using static Domain.Common.Constants;

namespace Infrastructure.DataAccessManager.EFCore.Configurations;

public class FileImageConfiguration : BaseEntityConfiguration<FileImage>
{
    public override void Configure(EntityTypeBuilder<FileImage> builder)
    {
        base.Configure(builder);

        builder.Property(e => e.Name).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(x => x.Description).HasMaxLength(DescriptionConsts.MaxLength).IsRequired(false);
        builder.Property(e => e.OriginalName).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(e => e.GeneratedName).HasMaxLength(NameConsts.MaxLength).IsRequired(false);
        builder.Property(e => e.Extension).HasMaxLength(CodeConsts.MaxLength).IsRequired(false);
        builder.Property(e => e.FileSize).IsRequired(false);

        builder.HasIndex(e => e.OriginalName);
        builder.HasIndex(e => e.GeneratedName);
    }
}
