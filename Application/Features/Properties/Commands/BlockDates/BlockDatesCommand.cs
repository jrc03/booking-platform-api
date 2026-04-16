using MediatR;

namespace Application.Features.Properties.Commands.BlockDates;

public record BlockDatesCommand(
    Guid PropertyId,
    DateTime StartDate,
    DateTime EndDate
) : IRequest<Guid>;
