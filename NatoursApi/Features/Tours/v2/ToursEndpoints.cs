namespace NatoursApi.Features.Tours.v2;

public static class ToursEndpoints
{
    public static void MapToursEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("tours", () => Results.Ok("Tours list")).MapToApiVersion(2.0);
    }
}