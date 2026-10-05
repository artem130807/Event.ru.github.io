using EventRep.Domain.Enums;
using FluentResults;

public class EventEntity
{
    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public Guid? ExecutorId {get; private set;}
    public Guid CustomerId {get; private set;}
    public EventStatus Status {get; private set;}
    public TimeSpan TimeStart {get; private set;}
    public TimeSpan TimeEnd {get; private set;}
    public DateTime CreatedAt {get; private set;}
    
    private EventEntity(){}
    public static Result<EventEntity> Create(string name, Guid? executorId, Guid customerId,TimeSpan timeStart, TimeSpan timeEnd)
    {
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            ExecutorId = executorId,
            CustomerId = customerId,
            TimeStart = timeStart,
            TimeEnd = timeEnd,
            CreatedAt = DateTime.UtcNow  
        };
        return eventEntity;
    }
    public void UpdateTimeStart(TimeSpan timeStart) => TimeStart = timeStart;
    public void UpdateTimeEnd(TimeSpan timeEnd) => TimeEnd = timeEnd;
    public void ToRefuseExecutor() => ExecutorId = null;
}