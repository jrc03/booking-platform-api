using System;
using System.Threading;
using System.Threading.Tasks;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Properties.Commands.DeleteProperty
{
    public class DeletePropertyCommandHandler : IRequestHandler<DeletePropertyCommand>
    {
        private readonly IPropertyRepository _propertyRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeletePropertyCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork)
        {
            _propertyRepository = propertyRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(DeletePropertyCommand request, CancellationToken cancellationToken)
        {
            var property = await _propertyRepository.GetByIdAsync(request.Id);
            
            if (property == null)
            {
                throw new Exception("Property not found");
            }

            _propertyRepository.Delete(property);
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            return; // MediatR retorna Unit automáticamente en IRequestHandler<TRequest> con IRequest (sin tipo de retorno).
        }
    }
}