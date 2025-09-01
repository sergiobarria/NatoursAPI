using Asp.Versioning;
using FluentValidation;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using NatoursApi.Api.V1.Dtos;
using NatoursApi.Domain.Entities;
using NatoursApi.Services.Abstractions;

namespace NatoursApi.Api.V1.Controllers;

[Route("api/v{version:apiVersion}/tours/{tourId:guid}/start-dates")]
[ApiController]
[ApiVersion(1.0)]
public class TourStartDatesController(ITourStartDateService startDateService)
    : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourStartDateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<List<TourStartDateDto>>> GetTourStartDates(Guid tourId, CancellationToken ct)
    {
        var entities = await startDateService.GetAllForTourAsync(tourId, ct);
        var startDates = entities.Adapt<List<TourStartDateDto>>();

        return Ok(startDates);
    }

    [HttpGet("{dateId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourStartDateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourStartDateDto>> GetTourStartDate(Guid tourId, Guid dateId,
        CancellationToken ct)
    {
        var entity = await startDateService.GetByIdForTourAsync(tourId, dateId, ct);
        var startDateDto = entity.Adapt<TourStartDateDto>();

        return Ok(startDateDto);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TourStartDateDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TourStartDateDto>> CreateTourStartDate(
        Guid tourId,
        CreateTourStartDateDto newStartDate,
        IValidator<CreateTourStartDateDto> validator,
        CancellationToken ct
    )
    {
        await validator.ValidateAndThrowAsync(newStartDate, ct);

        var startDateEntity = newStartDate.Adapt<TourStartDate>();
        var createdStartDate = await startDateService.CreateForTourAsync(tourId, startDateEntity, ct);
        var startDateDto = createdStartDate.Adapt<TourStartDateDto>();

        return CreatedAtAction(nameof(GetTourStartDate), new { tourId, dateId = startDateDto.Id, version = "1.0" },
            startDateDto);
    }

    [HttpPut("{dateId:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TourStartDateDto))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TourStartDateDto>> UpdateTourStartDate(Guid tourId,
        CreateTourStartDateDto updateStartDateDto, IValidator<CreateTourStartDateDto> validator, CancellationToken ct)
    {
        await validator.ValidateAndThrowAsync(updateStartDateDto, ct);

        var updatedStartDateEntity = updateStartDateDto.Adapt<TourStartDate>();

        var resultStartDate = await startDateService.UpdateForTourAsync(tourId, updatedStartDateEntity, ct);
        var resultStartDateDto = resultStartDate.Adapt<TourStartDateDto>();

        return Ok(resultStartDateDto);
    }

    [HttpDelete("{dateId:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteTourStartDate(Guid tourId, Guid dateId, CancellationToken ct)
    {
        await startDateService.DeleteForTourAsync(tourId, dateId, ct);
        return NoContent();
    }
}