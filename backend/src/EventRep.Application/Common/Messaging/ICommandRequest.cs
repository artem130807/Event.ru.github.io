namespace EventRep.Application.Common.Messaging;

/// <summary>
/// Marks a MediatR request as a state-changing operation.
/// </summary>
public interface ICommandRequest;

/// <summary>
/// Marks a command that requires an explicit database transaction.
/// </summary>
public interface ITransactionalRequest : ICommandRequest;
