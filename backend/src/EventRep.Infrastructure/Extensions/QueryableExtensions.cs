using EventRep.Domain.Contracts.Pagination;
using Microsoft.EntityFrameworkCore;

namespace EventRep.Infrastructure.Extensions;

public static class QueryableExtensions
{
    public static async Task<PagedResult<T>> ToPagedAsync<T>(
        this IQueryable<T> query,
        PageParams pageParams,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(query);
        ArgumentNullException.ThrowIfNull(pageParams);

        var totalCount = await query.CountAsync(cancellationToken);
        var items = await query
            .Skip(pageParams.Skip)
            .Take(pageParams.PageSize)
            .ToListAsync(cancellationToken);

        return new PagedResult<T>(items, totalCount, pageParams);
    }
}
