namespace Shared.RequestFeatures;

public class PagedList<T>
{
    public IEnumerable<T> Items { get; set; } = new List<T>();

    public PaginationMetadata Metadata { get; set; } = null!;
}