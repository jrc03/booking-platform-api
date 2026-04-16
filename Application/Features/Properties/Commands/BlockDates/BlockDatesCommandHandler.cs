using Application.Interfaces.Auth;
using Domain.Interfaces;
using Domain.ValueObjects;
using MediatR;

namespace Application.Features.Properties.Commands.BlockDates;

public class BlockDatesCommandHandler : IRequestHandler<BlockDatesCommand, Guid>
{
    private readonly IPropertyRepository _propertyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ICurrentUserService _currentUserService;

    public BlockDatesCommandHandler(
        IPropertyRepository propertyRepository,
        IUnitOfWork unitOfWork,
        ICurrentUserService currentUserService)
    {
        _propertyRepository = propertyRepository;
        _unitOfWork = unitOfWork;
        _currentUserService = currentUserService;
    }

    public async Task<Guid> Handle(BlockDatesCommand request, CancellationToken cancellationToken)
    {
        var property = await _propertyRepository.GetByIdAsync(request.PropertyId);

        if (property == null)
            throw new Exception("Property not found."); 

        if (property.HostId != _currentUserService.UserId)
            throw new UnauthorizedAccessException("You can only block dates on your own properties.");

        var dateRange = new DateRange(request.StartDate, request.EndDate);
        property.BlockDateRange(dateRange);

        _propertyRepository.Update(property);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return property.Id;
    }
}
