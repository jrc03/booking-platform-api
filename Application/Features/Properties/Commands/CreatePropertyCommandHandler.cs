using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Commands
{
    public class CreatePropertyCommandHandler : IRequestHandler<CreatePropertyCommand, PropertyResponseDto>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreatePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PropertyResponseDto> Handle(CreatePropertyCommand request, CancellationToken cancellationToken)
        {
            var newProperty = new Property
            {
                Title = request.Title,
                Description = request.Description,
                Location = request.Location,
                Capacity = request.Capacity,
                PricePerNight = request.PricePerNight,
                HostId = request.HostId 
            };

            _propertyRepository.Add(newProperty);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new PropertyResponseDto
            (
               newProperty.Id,
               newProperty.Title,
               newProperty.Description,
               newProperty.Location,
               newProperty.Capacity,
               newProperty.PricePerNight
               );


        }
    }
}