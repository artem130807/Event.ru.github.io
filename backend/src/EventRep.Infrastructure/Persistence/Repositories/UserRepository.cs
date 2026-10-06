using EventRep.Domain.Contracts;
using EventRep.Domain.ValueObjects;
using EventRep.Infrastructure.Persistence.Mappers;
using EventRep.Infrastructure.Persistence.Models;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Persistence.Repositories;

internal sealed class UserRepository(EventRepDbContext dbContext)
    : Repository<UserEntity, UserOrm>(dbContext), IUserRepository
{
    protected override UserEntity ToDomain(UserOrm model) => model.ToDomain();

    protected override UserOrm ToOrm(UserEntity entity) => entity.ToOrm();

    public Task<UserEntity?> GetByPhoneNumberAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return ToDomainSingleOrDefaultAsync(
            Set.Where(user => user.PhoneNumber == phoneNumber.Value),
            cancellationToken);
    }

    public Task<bool> ExistsByPhoneNumberAsync(
        PhoneNumber phoneNumber,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(phoneNumber);

        return Set.AnyAsync(
            user => user.PhoneNumber == phoneNumber.Value,
            cancellationToken);
    }
}
