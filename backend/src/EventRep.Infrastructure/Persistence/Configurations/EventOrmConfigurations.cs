using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class EventOrmConfigurations : IEntityTypeConfiguration<EventOrm>
{
    public void Configure(EntityTypeBuilder<EventOrm> builder)
    {
        builder.ToTable("events");

        builder.HasKey(eventOrm => eventOrm.Id);

        builder.Property(eventOrm => eventOrm.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(eventOrm => eventOrm.Name)
            .HasColumnName("name")
            .HasMaxLength(200)
            .IsRequired();

        builder.Property(eventOrm => eventOrm.ExecutorId)
            .HasColumnName("executor_id");

        builder.Property(eventOrm => eventOrm.CustomerId)
            .HasColumnName("customer_id")
            .IsRequired();

        builder.Property(eventOrm => eventOrm.Status)
            .HasColumnName("status")
            .HasConversion<string>()
            .HasMaxLength(32)
            .IsRequired();

        builder.Property(eventOrm => eventOrm.TimeStart)
            .HasColumnName("time_start")
            .HasColumnType("time without time zone")
            .IsRequired();

        builder.Property(eventOrm => eventOrm.TimeEnd)
            .HasColumnName("time_end")
            .HasColumnType("time without time zone")
            .IsRequired();

        builder.Property(eventOrm => eventOrm.FullDate)
            .HasColumnName("full_date")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.Property(eventOrm => eventOrm.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(eventOrm => eventOrm.Customer)
            .WithMany(user => user.CustomerEvents)
            .HasForeignKey(eventOrm => eventOrm.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(eventOrm => eventOrm.Executor)
            .WithMany(user => user.ExecutedEvents)
            .HasForeignKey(eventOrm => eventOrm.ExecutorId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.HasIndex(eventOrm => eventOrm.CustomerId)
            .HasDatabaseName("ix_events_customer_id");

        builder.HasIndex(eventOrm => eventOrm.ExecutorId)
            .HasDatabaseName("ix_events_executor_id");

        builder.HasIndex(eventOrm => new { eventOrm.Status, eventOrm.FullDate })
            .HasDatabaseName("ix_events_status_full_date");
    }
}
