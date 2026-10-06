using EventRep.Domain.ValueObjects;

namespace EventRep.Domain.Contracts;

public interface IUserRepository : IRepository<UserEntity>
{
    Task<UserEntity?> GetByPhoneNumberAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);

    Task<bool> ExistsByPhoneNumberAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default);
}
