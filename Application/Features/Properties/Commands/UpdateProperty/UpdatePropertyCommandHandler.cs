using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Application.Interfaces.Auth;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyResponseDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PropertyResponseDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var hostId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not logged in.");
            var property = await _propertyRepository.GetByIdAsync(request.Id) ?? throw new Exception("Property not found");

            if (property.HostId != hostId) 
                throw new UnauthorizedAccessException("You are not the owner of this property.");

            property.UpdateDetails(
                request.Title,
                request.Description,
                request.Location,
                request.PricePerNight,
                request.Capacity
            );

            _propertyRepository.Update(property);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PropertyResponseDto(
                property.Id,
                property.Title,
                property.Description,
                property.Location,
                property.Capacity,
                property.PricePerNight
            );
        }
    }
}
