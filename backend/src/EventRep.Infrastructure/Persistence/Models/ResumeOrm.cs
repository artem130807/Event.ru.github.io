namespace EventRep.Infrastructure.Persistence.Models;

public sealed class ResumeOrm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Guid? PortfolioEntityId { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserOrm User { get; set; } = null!;
    public PortfolioOrm? Portfolio { get; set; }
}
