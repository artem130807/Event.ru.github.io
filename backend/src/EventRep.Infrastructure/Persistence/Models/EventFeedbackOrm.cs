namespace EventRep.Infrastructure.Persistence.Models;

public sealed class EventFeedbackOrm
{
    public Guid Id { get; set; }
    public Guid EventId { get; set; }
    public Guid ExecutorId { get; set; }
    public DateTime CreatedAt { get; set; }

    public EventOrm Event { get; set; } = null!;
    public UserOrm Executor { get; set; } = null!;
}
