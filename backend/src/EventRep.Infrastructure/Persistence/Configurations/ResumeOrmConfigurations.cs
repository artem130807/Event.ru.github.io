using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class ResumeOrmConfigurations : IEntityTypeConfiguration<ResumeOrm>
{
    public void Configure(EntityTypeBuilder<ResumeOrm> builder)
    {
        builder.ToTable("resumes");

        builder.HasKey(resume => resume.Id);

        builder.Property(resume => resume.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(resume => resume.Name)
            .HasColumnName("name")
            .HasMaxLength(150)
            .IsRequired();

        builder.Property(resume => resume.Description)
            .HasColumnName("description")
            .HasMaxLength(4_000)
            .IsRequired();

        builder.Property(resume => resume.PortfolioEntityId)
            .HasColumnName("portfolio_id");

        builder.Property(resume => resume.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(resume => resume.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(resume => resume.UpdatedAt)
            .HasColumnName("updated_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(resume => resume.User)
            .WithOne(user => user.Resume)
            .HasForeignKey<ResumeOrm>(resume => resume.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(resume => resume.Portfolio)
            .WithOne(portfolio => portfolio.Resume)
            .HasForeignKey<ResumeOrm>(resume => resume.PortfolioEntityId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(resume => resume.UserId)
            .IsUnique()
            .HasDatabaseName("ux_resumes_user_id");

        builder.HasIndex(resume => resume.PortfolioEntityId)
            .IsUnique()
            .HasDatabaseName("ux_resumes_portfolio_id");
    }
}
