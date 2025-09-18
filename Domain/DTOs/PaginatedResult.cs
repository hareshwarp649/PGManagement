namespace bca.api.DTOs
{
    public class PaginatedResult<T>
    {
        public required List<T> Items { get; set; }
        public int TotalRecords { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
