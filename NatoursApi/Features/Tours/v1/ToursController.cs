using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace NatoursApi.Features.Tours.v1;

[ApiController]
[Route("api/v{version:apiVersion}/tours")]
[ApiVersion(1.0)]
public class ToursController(ITourService tourService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(List<TourDto>))]
    public async Task<ActionResult<List<TourDto>>> GetTours(CancellationToken ct)
    {
        var tours = await tourService.GetAllAsync(ct);

        return Ok(tours);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TourDto>> GetTourById(Guid? id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateTour()
    {
        throw new NotImplementedException();
    }

    [HttpPut("{id:guid}")]
    public async Task<ActionResult<TourDto>> UpdateTour(Guid? id, string? name, CancellationToken ct)
    {
        throw new NotImplementedException();
    }

    [HttpDelete("{id:guid}")]
    public async Task<ActionResult<TourDto>> DeleteTour(Guid? id, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}