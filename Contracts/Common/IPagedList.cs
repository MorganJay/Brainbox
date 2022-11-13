namespace Contracts.Common
{
    public interface IPagedList<T>
    {
        int CurrentPage { get; }
        bool HasNext { get; }
        bool HasPrevious { get; }
        List<T> Items { get; }
        int PageSize { get; }
        int TotalCount { get; }
        int TotalPages { get; }
    }
}