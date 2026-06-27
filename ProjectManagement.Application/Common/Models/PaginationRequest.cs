namespace ProjectManagement.Application.Common.Models
{
    public abstract class PaginationRequest
    {
        private const int _maxPageSize = 50;
        private int _pageNumber = 1;
        private int _pageSize = 10;

        public int PageNumber
        {
            get => _pageNumber;
            init => _pageNumber = value < 0 ? 1 : value;
        }

        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value switch
            {
                <= 0 => 10,
                > _maxPageSize => _maxPageSize,
                _ => value
            };
        }
    }
}
