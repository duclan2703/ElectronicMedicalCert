using ElectronicMedicalCert.Application.Contracts.V2;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CheckOpravneni;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.CreatePosudekRo;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Commands.InvalidatePosudekRo;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoDetail;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoHistorie;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.GetPosudekRoPdf;
using ElectronicMedicalCert.Application.Features.PosudkyRo.Queries.SearchPosudkyRo;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ElectronicMedicalCert.Controllers;

[ApiController]
[Authorize]
[Route("api/v2/posudky/ridicskeOpravneni")]
public sealed class RidicskeOpravneniController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Create([FromBody] PosudekRoCreateDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CreatePosudekRoCommand(dto), ct);
        return StatusCode(StatusCodes.Status201Created, result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetDetail([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPosudekRoDetailQuery(id), ct);
        if (!string.IsNullOrWhiteSpace(result.VerzeZaznamu))
        {
            Response.Headers["ETag"] = result.VerzeZaznamu;
        }
        return Ok(result);
    }

    [HttpPost("vyhledat")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromBody] PosudkyRoSearchRequest dto, CancellationToken ct)
    {
        var result = await mediator.Send(new SearchPosudkyRoQuery(dto), ct);
        return Ok(result);
    }

    [HttpGet("{id:guid}/historie")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetHistorie([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPosudekRoHistorieQuery(id), ct);
        return Ok(result);
    }

    [HttpPatch("{id:guid}/zneplatnit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Invalidate([FromRoute] Guid id, [FromHeader(Name = "If-Match")] string ifMatch, CancellationToken ct)
    {
        var result = await mediator.Send(new InvalidatePosudekRoCommand(id, ifMatch), ct);
        if (!string.IsNullOrWhiteSpace(result.VerzeZaznamu))
        {
            Response.Headers["ETag"] = result.VerzeZaznamu;
        }
        return Ok(result);
    }

    [HttpGet("{id:guid}/pdf")]
    [Produces("application/pdf")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetPdf([FromRoute] Guid id, CancellationToken ct)
    {
        var result = await mediator.Send(new GetPosudekRoPdfQuery(id), ct);
        return File(result.Content, "application/pdf", result.FileName);
    }

    [HttpPost("zalozeni/opravneni")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> CheckOpravneni([FromBody] PosudekRoOpravneniRequestDto dto, CancellationToken ct)
    {
        var result = await mediator.Send(new CheckOpravneniCommand(dto), ct);
        return Ok(result);
    }
}

