using System;
using FluentResults;

namespace EventRep.Domain.Entity;

public class PortfolioEntity
{
    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public Guid UserId {get; private set;}
    public int CountWorks {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime UpdatedAt {get; private set;}

    private PortfolioEntity()
    {
        Name = null!;
    }

    internal static PortfolioEntity Rehydrate(
        Guid id,
        string name,
        Guid userId,
        int countWorks,
        DateTime createdAt,
        DateTime updatedAt) =>
        new()
        {
            Id = id,
            Name = name,
            UserId = userId,
            CountWorks = countWorks,
            CreatedAt = createdAt,
            UpdatedAt = updatedAt
        };

    public static Result<PortfolioEntity> Create(string name, Guid userId, int countWorks)
    {
        var portfolioEntity = new PortfolioEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            UserId = userId, 
            CountWorks = countWorks,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return portfolioEntity;
    }
    public void AddCountWorks() => CountWorks++;
    public void RemoveCountWorks() => CountWorks--;
    public void UpdateName(string name) => Name = name;

}
