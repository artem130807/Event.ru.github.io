using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class PortfolioOrmConfigurations : IEntityTypeConfiguration<PortfolioOrm>
{
    public void Configure(EntityTypeBuilder<PortfolioOrm> builder)
    {
        builder.ToTable("portfolios", table =>
            table.HasCheckConstraint("ck_portfolios_count_works", "count_works >= 0"));

        builder.HasKey(portfolio => portfolio.Id);

        builder.Property(portfolio => portfolio.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(portfolio => portfolio.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(portfolio => portfolio.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(portfolio => portfolio.CountWorks)
            .HasColumnName("count_works")
            .IsRequired();

        builder.Property(portfolio => portfolio.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(portfolio => portfolio.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(portfolio => portfolio.User)
            .WithOne(user => user.Portfolio)
            .HasForeignKey<PortfolioOrm>(portfolio => portfolio.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(portfolio => portfolio.UserId)
            .IsUnique()
            .HasDatabaseName("ux_portfolios_user_id");
    }
}
