namespace EventRep.Domain.Contracts;

public interface IUnitOfWork : IAsyncDisposable
{
    bool HasActiveTransaction { get; }

    IUserRepository Users { get; }

    IEventRepository Events { get; }

    IEventFeedbackRepository EventFeedbacks { get; }

    IPortfolioRepository Portfolios { get; }

    IPortfolioElementRepository PortfolioElements { get; }

    IResumeRepository Resumes { get; }

    Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default);

    Task BeginTransactionAsync(
        CancellationToken cancellationToken = default);

    Task CommitAsync(
        CancellationToken cancellationToken = default);

    Task RollbackAsync(
        CancellationToken cancellationToken = default);
}
