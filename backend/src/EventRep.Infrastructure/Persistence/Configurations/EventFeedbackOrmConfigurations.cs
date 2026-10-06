using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EventRep.Infrastructure.Persistence.Configurations;

public sealed class EventFeedbackOrmConfigurations : IEntityTypeConfiguration<EventFeedbackOrm>
{
    public void Configure(EntityTypeBuilder<EventFeedbackOrm> builder)
    {
        builder.ToTable("event_feedbacks");

        builder.HasKey(feedback => feedback.Id);

        builder.Property(feedback => feedback.Id)
            .HasColumnName("id")
            .ValueGeneratedNever();

        builder.Property(feedback => feedback.EventId)
            .HasColumnName("event_id")
            .IsRequired();

        builder.Property(feedback => feedback.ExecutorId)
            .HasColumnName("executor_id")
            .IsRequired();

        builder.Property(feedback => feedback.CreatedAt)
            .HasColumnName("created_at")
            .HasColumnType("timestamp with time zone")
            .IsRequired();

        builder.HasOne(feedback => feedback.Event)
            .WithMany(eventOrm => eventOrm.Feedbacks)
            .HasForeignKey(feedback => feedback.EventId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(feedback => feedback.Executor)
            .WithMany(user => user.EventFeedbacks)
            .HasForeignKey(feedback => feedback.ExecutorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(feedback => new { feedback.EventId, feedback.ExecutorId })
            .IsUnique()
            .HasDatabaseName("ux_event_feedbacks_event_executor");

        builder.HasIndex(feedback => feedback.ExecutorId)
            .HasDatabaseName("ix_event_feedbacks_executor_id");
    }
}
