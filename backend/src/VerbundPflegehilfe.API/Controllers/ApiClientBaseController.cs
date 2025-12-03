using MediatR;
using Microsoft.AspNetCore.Mvc;
using VerbundPflegehilfe.Application.Common.Models;

namespace VerbundPflegehilfe.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiClientBaseController : ControllerBase
{
    private ISender _mediator;
    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();

    protected ActionResult<Result<T>> HandleResult<T>(Result<T> result)
    {
        if (result.Succeeded)
        {
            return Ok(result);
        }

        return BadRequest(result);
    }
}