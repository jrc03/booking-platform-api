using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Features.Properties.DTOs;
using Application.Interfaces.Auth;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Queries.GetMyProperties
{
    public class GetMyPropertiesQueryHandler : IRequestHandler<GetMyPropertiesQuery, IEnumerable<PropertyResponseDto>>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly ICurrentUserService _currentUserService;

        public GetMyPropertiesQueryHandler(IPropertyRepository propertyRepository, ICurrentUserService currentUserService)
        {
            _propertyRepository = propertyRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<PropertyResponseDto>> Handle(GetMyPropertiesQuery request, CancellationToken cancellationToken)
        {
            var hostId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Log in required.");

            var properties = await _propertyRepository.GetByHostIdAsync(hostId);

            return properties.Select(p => new PropertyResponseDto(
                p.Id,
                p.Title,
                p.Description,
                p.Location,
                p.Capacity,
                p.PricePerNight,
                p.ImageUrls
            ));
        }
    }
}