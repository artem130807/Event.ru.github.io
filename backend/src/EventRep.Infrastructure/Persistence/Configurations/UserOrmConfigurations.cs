using EventRep.Domain.ValueObjects;
using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class UserOrmConfigurations : IEntityTypeConfiguration<UserOrm>
{
    public void Configure(EntityTypeBuilder<UserOrm> builder)
    {
        builder.ToTable("users", table =>
            table.HasCheckConstraint("ck_users_age", "age BETWEEN 0 AND 150"));

        builder.HasKey(user => user.Id);

        builder.Property(user => user.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(user => user.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(user => user.Age)
            .HasColumnName("age")
            .HasColumnType("smallint")
            .IsRequired();

        builder.Property(user => user.Gender)
            .HasColumnName("gender")
            .HasConversion<string>()
            .HasMaxLength(16)
            .IsRequired();

        builder.Property(user => user.PasswordHash)
            .HasColumnName("password_hash")
            .HasMaxLength(512)
            .IsRequired();

        builder.Property(user => user.PhoneNumber)
            .HasColumnName("phone_number")
            .HasMaxLength(PhoneNumber.CanonicalLength)
            .IsRequired();

        builder.Property(user => user.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasIndex(user => user.PhoneNumber)
            .IsUnique()
            .HasDatabaseName("ux_users_phone_number");

        builder.HasIndex(user => user.CreatedAt)
            .HasDatabaseName("ix_users_created_at");
    }
}
