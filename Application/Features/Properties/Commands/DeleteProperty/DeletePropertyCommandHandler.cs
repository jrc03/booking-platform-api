using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces.Auth;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeletePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var hostId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("User not logged in.");
            var property = await _propertyRepository.GetByIdAsync(request.Id) ?? throw new Exception("Property not found");

            if (property.HostId != hostId) 
                throw new UnauthorizedAccessException("You are not the owner of this property.");
            
            _propertyRepository.Delete(property);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return; // MediatR returns Unit automatically in IRequestHandler<TRequest> with IRequest (no return type).
        }
    }
}
