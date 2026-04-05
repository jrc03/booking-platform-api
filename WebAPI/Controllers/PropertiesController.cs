using Application.Features.Properties;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Commands.DeleteProperty;
using Application.Features.Properties.Commands.UpdateProperty;
using Application.Features.Properties.Queries.GetAllProperties;
using Application.Features.Properties.Queries.GetPropertyById;
using Application.Features.Properties.Queries.SearchProperties;
using MediatR;
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
    // Este endpoint ejecuta nuestro GetAllPropertiesQuery. Devuelve una lista de DTOs.
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var result = await _sender.Send(new GetAllPropertiesQuery());
        return Ok(result);
    }

    // GET: api/properties/{id}
    // Extraemos el id (Guid) directo de la URL y construimos la consulta para buscarlo.
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await _sender.Send(new GetPropertyByIdQuery(id));
        return Ok(result);
    }

    // POST: api/properties
    // Cacha el JSON enviado por el host, lo valida automáticamente gracias al Pipeline de FluentValidation,
    // y nos va a devolver un "201 Created" junto con los datos de la nueva propiedad.
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreatePropertyCommand command)
    {
        var result = await _sender.Send(command);

        // Buena práctica HTTP: Devolver dónde se puede ubicar el nuevo recurso (GetById)
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    // PUT: api/properties/{id}
    // Actualizamos propiedades. El ID viene en la URL y los demás datos por el body.
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePropertyCommand command)
    {
        // Regla de seguridad simple: validar que no intenten hackear mandando un ID en la URL y otro en el Body
        if (id != command.Id)
            return BadRequest(new { error = "El ID de la URL y del body no coinciden." });

        var result = await _sender.Send(command);
        return Ok(result);
    }

    // DELETE: api/properties/{id}
    // El endpoint de borrado. Enviamos la orden de eliminar. Al ser un proceso sin "respuesta" en formato JSON,
    // devolver NoContent (Status 204) es el estándar de API REST mundial.
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeletePropertyCommand(id));
        return NoContent();
    }
}
