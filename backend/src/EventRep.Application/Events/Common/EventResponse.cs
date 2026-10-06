using EventRep.Domain.Enums;

namespace EventRep.Application.Events.Common;

public sealed record EventResponse(
    Guid Id,
    string Name,
    Guid CustomerId,
    Guid? ExecutorId,
    EventStatus Status,
    TimeSpan TimeStart,
    TimeSpan TimeEnd,
    DateTime FullDate,
    DateTime CreatedAt);
