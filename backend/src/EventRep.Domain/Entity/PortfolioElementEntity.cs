using System;
using FluentResults;

namespace EventRep.Domain.Entity;

public class PortfolioElementEntity
{
    public Guid Id {get; private set;}
    public Guid PortfolioId {get; private set;}
    public string Name {get; private set;}
    public string Description {get; private set;}
    public string S3Key {get; private set;}
    public string S3File {get; private set;}
    public DateTime CreatedAt {get; private set;}

    public static Result<PortfolioElementEntity> Create(Guid portfolioId, string name, string description, string s3Key, string s3File)
    {
        var portfolioElement = new PortfolioElementEntity
        {
            Id = Guid.NewGuid(),
            PortfolioId = portfolioId,
            Name = name,
            Description = description,
            S3Key = s3Key,
            S3File = s3File,
            CreatedAt = DateTime.UtcNow  
        };
        return portfolioElement;
    }
    public void Rename(string name) => Name = name;
    public void UpdateDescription(string description) => Description = description;
    public void UpdateS3File(string s3File) => S3File = s3File;
    public void UpdateS3Key(string s3Key) => S3Key = s3Key;
}
