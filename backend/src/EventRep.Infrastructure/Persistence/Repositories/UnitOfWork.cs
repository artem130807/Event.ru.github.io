using EventRep.Domain.Contracts;
using Microsoft.EntityFrameworkCore.Storage;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class UnitOfWork(
    EventRepDbContext dbContext,
    IUserRepository users,
    IEventRepository events,
    IEventFeedbackRepository eventFeedbacks,
    IPortfolioRepository portfolios,
    IPortfolioElementRepository portfolioElements,
    IResumeRepository resumes) : IUnitOfWork
{
    private IDbContextTransaction? _currentTransaction;

    public IUserRepository Users { get; } = users;

    public IEventRepository Events { get; } = events;

    public IEventFeedbackRepository EventFeedbacks { get; } = eventFeedbacks;

    public IPortfolioRepository Portfolios { get; } = portfolios;

    public IPortfolioElementRepository PortfolioElements { get; } = portfolioElements;

    public IResumeRepository Resumes { get; } = resumes;

    public Task<int> SaveChangesAsync(
        CancellationToken cancellationToken = default) =>
        dbContext.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(
        CancellationToken cancellationToken = default)
    {
        if (_currentTransaction is not null ||
            dbContext.Database.CurrentTransaction is not null)
        {
            throw new InvalidOperationException(
                "A database transaction is already active for this unit of work.");
        }

        _currentTransaction = await dbContext.Database
            .BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitAsync(
        CancellationToken cancellationToken = default)
    {
        var transaction = GetCurrentTransaction();

        try
        {
            await dbContext.SaveChangesAsync(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await RollbackAfterFailedCommitAsync(transaction);
            throw;
        }
        finally
        {
            await DisposeTransactionAsync(transaction);
        }
    }

    public async Task RollbackAsync(
        CancellationToken cancellationToken = default)
    {
        var transaction = GetCurrentTransaction();

        try
        {
            await transaction.RollbackAsync(cancellationToken);
        }
        finally
        {
            dbContext.ChangeTracker.Clear();
            await DisposeTransactionAsync(transaction);
        }
    }

    public async ValueTask DisposeAsync()
    {
        if (_currentTransaction is null)
        {
            return;
        }

        var transaction = _currentTransaction;

        try
        {
            await transaction.RollbackAsync(CancellationToken.None);
            dbContext.ChangeTracker.Clear();
        }
        finally
        {
            await DisposeTransactionAsync(transaction);
        }
    }

    private IDbContextTransaction GetCurrentTransaction() =>
        _currentTransaction
        ?? throw new InvalidOperationException(
            "No active database transaction exists for this unit of work.");

    private async Task RollbackAfterFailedCommitAsync(
        IDbContextTransaction transaction)
    {
        try
        {
            await transaction.RollbackAsync(CancellationToken.None);
        }
        finally
        {
            dbContext.ChangeTracker.Clear();
        }
    }

    private async ValueTask DisposeTransactionAsync(
        IDbContextTransaction transaction)
    {
        try
        {
            await transaction.DisposeAsync();
        }
        finally
        {
            if (ReferenceEquals(_currentTransaction, transaction))
            {
                _currentTransaction = null;
            }
        }
    }
}
