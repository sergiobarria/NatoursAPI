using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace NatoursApi.Api.V1.Controllers;

[Route("api/[controller]")]
[ApiVersion(1.0)]
[ApiController]
public class AuthenticationController : ControllerBase
{
}