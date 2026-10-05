using System;
using FluentResults;

namespace EventRep.Domain.Entity;

public class ResumeEntity
{
    public Guid Id {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    public Guid? PortfolioEntityId {get; private set;}
    public Guid UserId {get; private set;}
    public DateTime CreatedAt {get; private set;}
    public DateTime UpdatedAt {get; private set;}
    private ResumeEntity(){}
    public static Result<ResumeEntity> Create(string name, string description , Guid userId)
    {
        var resumeEntity = new ResumeEntity
        {
            Name = name,
            Description = description,
            PortfolioEntityId = null,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        return resumeEntity;
    }
    public void UpdateDescription(string description) => Description = description;
    public void UpdateName(string name) => Name = name;
    public void AddPortfolio(Guid portfolioId) => PortfolioEntityId = portfolioId;
}
