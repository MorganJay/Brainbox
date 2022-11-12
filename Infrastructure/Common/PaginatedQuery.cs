namespace Infrastructure.Common
{
    public class PaginatedQuery
    {
        public int Page { get; set; } = 1;
        public int Size { get; set; } = 20;
    }
}