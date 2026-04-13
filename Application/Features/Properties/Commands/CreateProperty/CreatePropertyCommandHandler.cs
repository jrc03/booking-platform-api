using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using Application.Features.Properties.DTOs;
using Application.Interfaces.Auth;

namespace Application.Features.Properties.Commands.CreateProperty
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyResponseDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<PropertyResponseDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var hostId = _currentUserService.UserId
             ?? throw new UnauthorizedAccessException("Invalid user.en el token.");

            var newProperty = Property.Create(
                hostId: hostId,
                title: request.Title,
                description: request.Description,
                location: request.Location,
                capacity: request.Capacity,
                pricePerNight: request.PricePerNight
            );

            if (request.ImageUrls != null)
            {
                foreach (var url in request.ImageUrls)
                {
                    newProperty.AddImage(url);
                }
            }

            _propertyRepository.Add(newProperty);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PropertyResponseDto
            (
               newProperty.Id,
               newProperty.Title,
               newProperty.Description,
               newProperty.Location,
               newProperty.Capacity,
               newProperty.PricePerNight,
               newProperty.ImageUrls
            );

        }
    }
}
