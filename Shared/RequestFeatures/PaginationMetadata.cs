namespace Shared.RequestFeatures;

public class PaginationMetadata(int totalItemCount, int pageNumber, int pageSize)
{
    public int TotalItemCount { get; set; } = totalItemCount;

    public int TotalPageCount { get; set; } = (int)Math.Ceiling(totalItemCount / (double)pageSize);
    public int PageSize { get; set; } = pageSize;
    public int CurrentPage { get; set; } = pageNumber;

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPageCount;
}