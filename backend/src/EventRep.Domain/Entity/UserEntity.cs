using EventRep.Domain.ValueObjects;
using FluentResults;

public class UserEntity
{
    public Guid Id {get; private set;}
    public string Name {get; private set; }
    public string PasswordHash {get; private set;}
    public PhoneNumber PhoneNumber {get; private set;}
    public DateTime CreatedAt {get; private set;}

    public static Result<UserEntity> Create(string name, string passwordHash, PhoneNumber phoneNumber)
    {
        var userEntity = new UserEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            CreatedAt = DateTime.UtcNow  
        };
        return userEntity;
    }
}