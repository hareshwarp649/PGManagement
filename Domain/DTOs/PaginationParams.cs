namespace bca.api.DTOs
{
    public class PaginationParams
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        // Optional: Max limit for page size
        private const int MaxPageSize = 50;

        public int Skip => (PageNumber - 1) * PageSize;

        public void Validate()
        {
            if (PageSize > MaxPageSize)
                PageSize = MaxPageSize;

            if (PageNumber <= 0)
                PageNumber = 1;
        }
    }
}
