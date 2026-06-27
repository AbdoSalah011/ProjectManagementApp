namespace ProjectManagement.Application.Common.Models
{
    public sealed class PaginatedList<T>
    {
        public IReadOnlyCollection<T> Items { get; }
        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        private PaginatedList(
                IReadOnlyCollection<T> items,
                int pageNumber,
                int pageSize,
                int totalCount
            )
        {
            Items = items;
            PageNumber = pageNumber;
            PageSize = PageSize;
            TotalCount = totalCount;
            TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize);
        }

        public async static Task<PaginatedList<T>> CreatAsync(
            IQueryable<T> source,
            int pageNumber,
            int pageSize,
            CancellationToken ct = default)
        {
            var totalCount = await source.CountAsync(ct);

            var items = await source
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(ct);

            return new PaginatedList<T>(
                items,
                pageNumber,
                pageSize,
                totalCount);
        }
    }
}
