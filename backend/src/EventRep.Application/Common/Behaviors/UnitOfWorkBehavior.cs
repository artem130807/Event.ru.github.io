using EventRep.Application.Common.Messaging;
using EventRep.Domain.Contracts;
using FluentResults;
using MediatR;

namespace EventRep.Application.Common.Behaviors;

internal sealed class UnitOfWorkBehavior<TRequest, TResponse>(
    IUnitOfWork unitOfWork)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        if (request is not ICommandRequest)
            return await next();

        var isTransactional = request is ITransactionalRequest;

        if (isTransactional)
            await unitOfWork.BeginTransactionAsync(cancellationToken);

        try
        {
            var response = await next();

            if (response is IResultBase { IsFailed: true })
            {
                if (unitOfWork.HasActiveTransaction)
                    await unitOfWork.RollbackAsync(cancellationToken);

                return response;
            }

            if (isTransactional)
                await unitOfWork.CommitAsync(cancellationToken);
            else
                await unitOfWork.SaveChangesAsync(cancellationToken);

            return response;
        }
        catch
        {
            if (unitOfWork.HasActiveTransaction)
                await unitOfWork.RollbackAsync(CancellationToken.None);

            throw;
        }
    }
}
