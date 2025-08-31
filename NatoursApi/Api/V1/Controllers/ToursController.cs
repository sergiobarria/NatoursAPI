using Asp.Versioning;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NatoursApi.Api.V1.Dtos;
using NatoursApi.Domain.Entities;
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

    [HttpGet("{id:guid}", Name = "GetTour")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourDto>> GetTourById(Guid id, CancellationToken ct)
    {
        var entity = await tourService.GetByIdAsync(id, ct);
        var tour = entity.Adapt<TourDto>();

        return Ok(tour);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TourDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> CreateTour(CreateTourDto newTourDto, IValidator<CreateTourDto> validator,
        CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(newTourDto, ct);

        var tourEntity = newTourDto.Adapt<Tour>();
        var newTour = await tourService.CreateAsync(tourEntity, ct);
        var tourDto = newTour.Adapt<TourDto>();

        return CreatedAtAction(nameof(GetTourById), new { id = tourDto.Id, version = "1.0" }, tourDto);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TourDto>> UpdateTour(Guid id, UpdateTourDto updateTourDto,
        IValidator<UpdateTourDto> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(updateTourDto, ct);

        var updatedTourEntity = updateTourDto.Adapt<Tour>();
        var updatedTour = await tourService.UpdateAsync(id, updatedTourEntity, ct);

        var updatedTourDto = updatedTour.Adapt<TourDto>();

        return Ok(updatedTourDto);
    }

    [HttpDelete("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourDto>> DeleteTour(Guid id, CancellationToken ct)
    {
        await tourService.DeleteAsync(id, ct);
        return NoContent();
    }
}