using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Bookings.Commands.CompleteBooking;
using Application.Features.Bookings.Commands.CreateBooking;
using Application.Features.Bookings.Queries.GetAllBookings;
using Application.Features.Bookings.Queries.GetBookingById;
using Application.Features.Bookings.Queries.GetBookingsByGuest;
using Application.Interfaces.Auth;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ISender _sender;
        private readonly ICurrentUserService _currentUserService;

        public BookingsController(ISender sender, ICurrentUserService currentUserService)
        {
            _sender = sender;
            _currentUserService = currentUserService;
        }

        // GET: api/bookings
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _sender.Send(new GetAllBookingsQuery());
            return Ok(result);
        }

        // GET: api/bookings/{id}
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var result = await _sender.Send(new GetBookingByIdQuery(id));
            if (result == null) return NotFound();
            return Ok(result);
        }

        // GET: api/bookings/guest
        [HttpGet("guest")]
        public async Task<IActionResult> GetMyBookings()
        {
            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");
            var result = await _sender.Send(new GetBookingsByGuestQuery(guestId));
            return Ok(result);
        }

        // POST: api/bookings
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateBookingCommand command)
        {
            var result = await _sender.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
        }

        // POST: api/bookings/{id}/cancel
        [HttpPost("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel(Guid id, [FromBody] CancelBookingCommand command)
        {
            if (id != command.BookingId)
                return BadRequest(new { error = "El ID de la URL y del body no coinciden." });

            var result = await _sender.Send(command);
            return Ok(new { success = result, message = "Reserva cancelada correctamente." });
        }

        // POST: api/bookings/{id}/complete
        [HttpPost("{id:guid}/complete")]
        public async Task<IActionResult> Complete(Guid id, [FromBody] CompleteBookingCommand command)
        {
            if (id != command.BookingId)
                return BadRequest(new { error = "El ID de la URL y del body no coinciden." });

            var result = await _sender.Send(command);
            return Ok(new { success = result, message = "Reserva marcada como completada." });
        }
    }
}
