namespace EventRep.Infrastructure.Persistence.Models;

public sealed class PortfolioOrm
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid UserId { get; set; }
    public int CountWorks { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public UserOrm User { get; set; } = null!;
    public List<PortfolioElementOrm> Elements { get; set; } = [];
    public ResumeOrm? Resume { get; set; }
}
