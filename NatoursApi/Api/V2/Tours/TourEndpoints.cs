namespace NatoursApi.Endpoints.V2;

public static class TourEndpoints
{
    public static void MapToursEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("tours", () => Results.Ok("Tours list")).MapToApiVersion(2.0);
    }
}