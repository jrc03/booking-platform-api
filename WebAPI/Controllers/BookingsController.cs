using Application.Features.Bookings.Commands.CancelBooking;
using Application.Features.Bookings.Commands.CompleteBooking;
using Application.Features.Bookings.Commands.CreateBooking;
using Application.Features.Bookings.Queries.GetAllBookings;
using Application.Features.Bookings.Queries.GetBookingById;
using Application.Features.Bookings.Queries.GetBookingsByGuest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookingsController : ControllerBase
    {
        private readonly ISender _sender;

        public BookingsController(ISender sender)
        {
            _sender = sender;
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

        // GET: api/bookings/guest/{guestId}
        [HttpGet("guest/{guestId:guid}")]
        public async Task<IActionResult> GetByGuestId(Guid guestId)
        {
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
        // Mandato: El cambio de estado debe realizarse por acciones específicas, no PUT/PATCH genérico.
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