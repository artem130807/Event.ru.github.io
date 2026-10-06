using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class PortfolioElementOrmConfigurations : IEntityTypeConfiguration<PortfolioElementOrm>
{
    public void Configure(EntityTypeBuilder<PortfolioElementOrm> builder)
    {
        builder.ToTable("portfolio_elements");

        builder.HasKey(element => element.Id);

        builder.Property(element => element.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(element => element.PortfolioId)
            .HasColumnName("portfolio_id")
            .IsRequired();

        builder.Property(element => element.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(element => element.Description)
            .HasColumnName("description")
            .HasMaxLength(4_000)
            .IsRequired();

        builder.Property(element => element.S3Key)
            .HasColumnName("s3_key")
            .HasMaxLength(1_024)
            .IsRequired();

        builder.Property(element => element.S3File)
            .HasColumnName("s3_file")
            .HasMaxLength(2_048)
            .IsRequired();

        builder.Property(element => element.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(element => element.Portfolio)
            .WithMany(portfolio => portfolio.Elements)
            .HasForeignKey(element => element.PortfolioId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(element => element.PortfolioId)
            .HasDatabaseName("ix_portfolio_elements_portfolio_id");

        builder.HasIndex(element => element.S3Key)
            .IsUnique()
            .HasDatabaseName("ux_portfolio_elements_s3_key");
    }
}
