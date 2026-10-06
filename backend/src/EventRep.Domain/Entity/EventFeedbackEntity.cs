using System;
using FluentResults;

namespace EventRep.Domain.Entity;

public class EventFeedbackEntity
{
    public Guid Id {get; private set;}
    public Guid EventId {get; private set;}
    public Guid ExecutorId {get; private set;}
    public DateTime CreatedAt {get; private set;}

    private EventFeedbackEntity(){}

    internal static EventFeedbackEntity Rehydrate(
        Guid id,
        Guid eventId,
        Guid executorId,
        DateTime createdAt) =>
        new()
        {
            Id = id,
            EventId = eventId,
            ExecutorId = executorId,
            CreatedAt = createdAt
        };

    public static Result<EventFeedbackEntity> Create(Guid eventId, Guid executorId)
    {
        var eventFeedBack = new EventFeedbackEntity
        {
            Id = Guid.NewGuid(),
            EventId = eventId,
            ExecutorId = executorId,
            CreatedAt = DateTime.UtcNow  
        };
        return eventFeedBack;
    }
}
