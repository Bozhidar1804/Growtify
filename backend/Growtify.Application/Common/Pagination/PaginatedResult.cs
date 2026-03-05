namespace Growtify.Application.Common.Pagination
{
    public class PaginatedResult<T>
    {
        public PaginationMetadata Metadata { get; set; } = default!;
        public List<T> Items { get; set; } = [];
    };
}
