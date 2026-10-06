using EventRep.Domain.Enums;
using EventRep.Domain.ValueObjects;
using FluentResults;

public class UserEntity
{
    public Guid Id {get; private set;}
    public string Name {get; private set; }
    public int Age {get; private set;}
    public Gender Gender {get; private set;}
    public string PasswordHash {get; private set;}
    public PhoneNumber PhoneNumber {get; private set;}
    public DateTime CreatedAt {get; private set;}

    private UserEntity()
    {
        Name = null!;
        PasswordHash = null!;
        PhoneNumber = null!;
    }

    internal static UserEntity Rehydrate(
        Guid id,
        string name,
        int age,
        Gender gender,
        string passwordHash,
        PhoneNumber phoneNumber,
        DateTime createdAt) =>
        new()
        {
            Id = id,
            Name = name,
            Age = age,
            Gender = gender,
            PasswordHash = passwordHash,
            PhoneNumber = phoneNumber,
            CreatedAt = createdAt
        };

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
    public void Rename(string name) => Name = name;
    public void UpdatePassword(string passwordHash) => PasswordHash = passwordHash;
}
