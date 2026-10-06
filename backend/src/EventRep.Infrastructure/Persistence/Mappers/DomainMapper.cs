using EventRep.Domain.Entity;
using EventRep.Domain.ValueObjects;
using EventRep.Infrastructure.Persistence.Models;

namespace EventRep.Infrastructure.Persistence.Mappers;

internal static class DomainMapper
{
    public static UserEntity ToDomain(this UserOrm model)
    {
        var phoneNumberResult = PhoneNumber.Create(model.PhoneNumber);

        if (phoneNumberResult.IsFailed)
        {
            throw new InvalidOperationException(
                $"В базе данных хранится некорректный номер пользователя {model.Id}: " +
                string.Join("; ", phoneNumberResult.Errors.Select(error => error.Message)));
        }

        return UserEntity.Rehydrate(
            model.Id,
            model.Name,
            model.Age,
            model.Gender,
            model.PasswordHash,
            phoneNumberResult.Value,
            model.CreatedAt);
    }

    public static UserOrm ToOrm(this UserEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Age = entity.Age,
            Gender = entity.Gender,
            PasswordHash = entity.PasswordHash,
            PhoneNumber = entity.PhoneNumber.Value,
            CreatedAt = entity.CreatedAt
        };

    public static EventEntity ToDomain(this EventOrm model) =>
        EventEntity.Rehydrate(
            model.Id,
            model.Name,
            model.ExecutorId,
            model.CustomerId,
            model.Status,
            model.TimeStart,
            model.TimeEnd,
            model.FullDate,
            model.CreatedAt);

    public static EventOrm ToOrm(this EventEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            ExecutorId = entity.ExecutorId,
            CustomerId = entity.CustomerId,
            Status = entity.Status,
            TimeStart = entity.TimeStart,
            TimeEnd = entity.TimeEnd,
            FullDate = entity.FullDate,
            CreatedAt = entity.CreatedAt
        };

    public static ResumeEntity ToDomain(this ResumeOrm model) =>
        ResumeEntity.Rehydrate(
            model.Id,
            model.Name,
            model.Description,
            model.PortfolioEntityId,
            model.UserId,
            model.CreatedAt,
            model.UpdatedAt);

    public static ResumeOrm ToOrm(this ResumeEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            Description = entity.Description,
            PortfolioEntityId = entity.PortfolioEntityId,
            UserId = entity.UserId,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

    public static PortfolioEntity ToDomain(this PortfolioOrm model) =>
        PortfolioEntity.Rehydrate(
            model.Id,
            model.Name,
            model.UserId,
            model.CountWorks,
            model.CreatedAt,
            model.UpdatedAt);

    public static PortfolioOrm ToOrm(this PortfolioEntity entity) =>
        new()
        {
            Id = entity.Id,
            Name = entity.Name,
            UserId = entity.UserId,
            CountWorks = entity.CountWorks,
            CreatedAt = entity.CreatedAt,
            UpdatedAt = entity.UpdatedAt
        };

    public static PortfolioElementEntity ToDomain(this PortfolioElementOrm model) =>
        PortfolioElementEntity.Rehydrate(
            model.Id,
            model.PortfolioId,
            model.Name,
            model.Description,
            model.S3Key,
            model.S3File,
            model.CreatedAt);

    public static PortfolioElementOrm ToOrm(this PortfolioElementEntity entity) =>
        new()
        {
            Id = entity.Id,
            PortfolioId = entity.PortfolioId,
            Name = entity.Name,
            Description = entity.Description,
            S3Key = entity.S3Key,
            S3File = entity.S3File,
            CreatedAt = entity.CreatedAt
        };

    public static EventFeedbackEntity ToDomain(this EventFeedbackOrm model) =>
        EventFeedbackEntity.Rehydrate(
            model.Id,
            model.EventId,
            model.ExecutorId,
            model.CreatedAt);

    public static EventFeedbackOrm ToOrm(this EventFeedbackEntity entity) =>
        new()
        {
            Id = entity.Id,
            EventId = entity.EventId,
            ExecutorId = entity.ExecutorId,
            CreatedAt = entity.CreatedAt
        };
}
