using EventRep.Domain.Enums;

namespace EventRep.Infrastructure.Persistence.Models;

public sealed class UserOrm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Age { get; set; }
    public Gender Gender { get; set; }
    public string PasswordHash { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }

    public List<EventOrm> CustomerEvents { get; set; } = [];
    public List<EventOrm> ExecutedEvents { get; set; } = [];
    public List<EventFeedbackOrm> EventFeedbacks { get; set; } = [];
    public PortfolioOrm? Portfolio { get; set; }
    public ResumeOrm? Resume { get; set; }
}
