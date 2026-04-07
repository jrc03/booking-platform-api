using Application.Features.Reviews.Commands.CreateReview;
using Application.Features.Reviews.Commands.DeleteReview;
using Application.Features.Reviews.Commands.UpdateReview;
using Application.Features.Reviews.Queries.GetReviewsByProperty;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ReviewsController : ControllerBase
{
    private readonly ISender _sender;

    public ReviewsController(ISender sender)
    {
        _sender = sender;
    }

    // GET: api/reviews/property/{propertyId}
    // Retrieves all reviews for a specific property, including the calculated average rating.
    [HttpGet("property/{propertyId:guid}")]
    public async Task<IActionResult> GetByProperty(Guid propertyId)
    {
        var result = await _sender.Send(new GetReviewsByPropertyQuery(propertyId));
        return Ok(result);
    }

    // POST: api/reviews
    // Creates a new review for a completed booking.
    [Authorize(Roles = "Guest")]
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewCommand command)
    {
        var result = await _sender.Send(command);
        return CreatedAtAction(nameof(GetByProperty), new { propertyId = result.PropertyId }, result);
    }

    // PUT: api/reviews/{id}
    // Updates an existing review's rating and/or comment payload.
    [Authorize(Roles = "Guest")]
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateReviewCommand command)
    {
        if (id != command.Id)
            return BadRequest(new { error = "Route ID and payload ID mismatch." });

        var result = await _sender.Send(command);
        return Ok(result);
    }

    // DELETE: api/reviews/{id}
    // Deletes a specific review. Ownership authorization uses JWT token directly.
    [Authorize(Roles = "Guest")]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        await _sender.Send(new DeleteReviewCommand(id));
        return NoContent(); // HTTP 204
    }
}