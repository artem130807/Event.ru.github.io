namespace EventRep.Domain.Contracts.Pagination;

public sealed class PagedResult<T>
{
    public PagedResult(
        List<T> items,
        int totalCount,
        PageParams pageParams)
    {
        ArgumentNullException.ThrowIfNull(items);
        ArgumentNullException.ThrowIfNull(pageParams);

        if (totalCount < 0)
            throw new ArgumentOutOfRangeException(
                nameof(totalCount),
                "Общее количество элементов не может быть отрицательным.");

        Items = items;
        TotalCount = totalCount;
        PageNumber = pageParams.PageNumber;
        PageSize = pageParams.PageSize;
        TotalPages = totalCount == 0
            ? 0
            : (int)Math.Ceiling(totalCount / (double)PageSize);
    }

    public List<T> Items { get; }

    public int TotalCount { get; }

    public int PageNumber { get; }

    public int PageSize { get; }

    public int TotalPages { get; }

    public bool HasPreviousPage => PageNumber > 1;

    public bool HasNextPage => PageNumber < TotalPages;
}
