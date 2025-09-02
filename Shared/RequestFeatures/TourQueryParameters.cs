namespace Shared.RequestFeatures;

public class TourQueryParameters : RequestQueryParameters
{
    public TourQueryParameters()
    {
        OrderBy = "name";
    }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public decimal MinPrice { get; set; }

    public decimal MaxPrice { get; set; } = int.MaxValue;

    public uint MinGroupSize { get; set; }

    public uint MaxGroupSize { get; set; } = int.MaxValue;

    public string Includes { get; set; } = string.Empty;
}