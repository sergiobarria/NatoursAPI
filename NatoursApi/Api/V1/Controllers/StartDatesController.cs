using Microsoft.AspNetCore.Mvc;

namespace NatoursApi.Api.V1.Controllers;

[Route("api/v{version:apiVersion}/tours/{id:guid}/start-dates")]
[ApiController]
public class StartDatesController : ControllerBase
{
}