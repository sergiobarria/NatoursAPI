using Asp.Versioning;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NatoursApi.Controllers.V1.Dtos;
using NatoursApi.Services.Abstractions;

namespace NatoursApi.Api.V1.Controllers;

[ApiController]
[Route("api/v{version:apiVersion}/tours")]
[ApiVersion(1.0)]
public class ToursController(ITourService tourService) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TourDto>))]
    public async Task<ActionResult<List<TourDto>>> GetTours(CancellationToken ct)
    {
        var entities = await tourService.GetAllAsync(ct);
        var tours = entities.Adapt<List<TourDto>>();

        return Ok(tours);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourDto>> GetTourById(Guid id, CancellationToken ct)
    {
        var entity = await tourService.GetByIdAsync(id, ct);
        var tour = entity.Adapt<TourDto>();

        return Ok(tour);
    }

    [HttpPost]
    public async Task<ActionResult<string>> CreateTour(CreateTourDto newTour, IValidator<CreateTourDto> validator)
    {
        await validator.ValidateAndThrowAsync(newTour);

        return Created("/api/v1/tours", newTour);
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