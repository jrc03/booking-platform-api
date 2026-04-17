using Application.Features.Properties;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Commands.DeleteProperty;
using Application.Features.Properties.Commands.UpdateProperty;
using Application.Features.Properties.Commands.BlockDates;
using Application.Features.Properties.Queries.GetAllProperties;
using Application.Features.Properties.Queries.GetMyProperties;
using Application.Features.Properties.Queries.GetPropertyById;
using Application.Features.Properties.Queries.GetUnavailableDates;
using Application.Features.Properties.Queries.SearchProperties;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PropertiesController : ControllerBase
{
    private readonly ISender _sender;

    public PropertiesController(ISender sender)
    {
        _sender = sender;
    }

    // GET: api/properties/search
    [HttpGet("search")]
    public async Task<IActionResult> Search([FromQuery] SearchPropertyQuery query)
    {
        var result = await _sender.Send(query);
        return Ok(result);
    }

    // GET: api/properties
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllPropertiesQuery());
        return Ok(result);
    }

    // GET: api/properties/{id}
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetPropertyByIdQuery(id));
        return Ok(result);
    }

    // GET: api/properties/{id}/unavailable-dates
    [HttpGet("{id:guid}/unavailable-dates")]
    public async Task<IActionResult> GetUnavailableDates(Guid id)
    {
        var result = await _sender.Send(new GetUnavailableDatesQuery(id));
        return Ok(result);
    }

    // GET: api/properties/my
    [Authorize(Roles = "Host")]
    [HttpGet("my")]
    public async Task<IActionResult> GetMyProperties()
    {
        var result = await _sender.Send(new GetMyPropertiesQuery());
        return Ok(result);
    }

    // POST: api/properties
    [Authorize(Roles = "Host")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyCommand command)
    {
        var result = await _sender.Send(command);

        // Buena práctica HTTP: Devolver dónde se puede ubicar el nuevo recurso (GetById)
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT: api/properties/{id}
    [Authorize(Roles = "Host")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "El ID de la URL y del body no coinciden." });

        var result = await _sender.Send(command);
        return Ok(result);
    }

    // DELETE: api/properties/{id}
    [Authorize(Roles = "Host")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeletePropertyCommand(id));
        return NoContent();
    }

    // POST: api/properties/{id}/block-dates
    [Authorize(Roles = "Host")]
    [HttpPost("{id:guid}/block-dates")]
    public async Task<IActionResult> BlockDates(Guid id, [FromBody] BlockDatesRequest request)
    {
        var command = new BlockDatesCommand(id, request.StartDate, request.EndDate);
        var result = await _sender.Send(command);
        return Ok(new { Message = "Dates blocked successfully.", PropertyId = result });
    }
}

public class BlockDatesRequest
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}
