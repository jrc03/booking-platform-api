using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Commands.UpdateProperty
{
    public class UpdatePropertyCommandHandler : IRequestHandler<UpdatePropertyCommand, PropertyResponseDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdatePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyResponseDto> Handle(UpdatePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.Id) ?? throw new Exception("Property not found");

    
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