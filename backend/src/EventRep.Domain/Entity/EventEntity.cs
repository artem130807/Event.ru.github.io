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
    public DateTime FullDate {get; private set;}
    public DateTime CreatedAt {get; private set;}
    
    private EventEntity()
    {
        Name = null!;
    }

    internal static EventEntity Rehydrate(
        Guid id,
        string name,
        Guid? executorId,
        Guid customerId,
        EventStatus status,
        TimeSpan timeStart,
        TimeSpan timeEnd,
        DateTime fullDate,
        DateTime createdAt) =>
        new()
        {
            Id = id,
            Name = name,
            ExecutorId = executorId,
            CustomerId = customerId,
            Status = status,
            TimeStart = timeStart,
            TimeEnd = timeEnd,
            FullDate = fullDate,
            CreatedAt = createdAt
        };

    public static Result<EventEntity> Create(string name, Guid? executorId, Guid customerId, TimeSpan timeStart, TimeSpan timeEnd, DateTime fullDate)
    {
        var eventEntity = new EventEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            ExecutorId = executorId,
            CustomerId = customerId,
            Status = EventStatus.Pending,
            TimeStart = timeStart,
            TimeEnd = timeEnd,
            CreatedAt = DateTime.UtcNow,
            FullDate = fullDate
        };
        return eventEntity;
    }
    public void UpdateDate(DateTime fullDate) => FullDate = fullDate;
    public void UpdateTimeStart(TimeSpan timeStart) => TimeStart = timeStart;
    public void UpdateTimeEnd(TimeSpan timeEnd) => TimeEnd = timeEnd;
    public void ToRefuseExecutor() => ExecutorId = null;
}
