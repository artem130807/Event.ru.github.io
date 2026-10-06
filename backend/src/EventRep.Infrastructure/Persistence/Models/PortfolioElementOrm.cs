namespace EventRep.Infrastructure.Persistence.Models;

public sealed class PortfolioElementOrm
{
    public Guid Id { get; set; }
    public Guid PortfolioId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string S3Key { get; set; } = string.Empty;
    public string S3File { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public PortfolioOrm Portfolio { get; set; } = null!;
}
