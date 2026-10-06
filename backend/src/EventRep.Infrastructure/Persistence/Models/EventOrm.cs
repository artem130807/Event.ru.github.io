using EventRep.Domain.Enums;

namespace EventRep.Infrastructure.Persistence.Models;

public sealed class EventOrm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? ExecutorId { get; set; }
    public Guid CustomerId { get; set; }
    public EventStatus Status { get; set; }
    public TimeSpan TimeStart { get; set; }
    public TimeSpan TimeEnd { get; set; }
    public DateTime FullDate { get; set; }
    public DateTime CreatedAt { get; set; }

    public UserOrm Customer { get; set; } = null!;
    public UserOrm? Executor { get; set; }
    public List<EventFeedbackOrm> Feedbacks { get; set; } = [];
}
