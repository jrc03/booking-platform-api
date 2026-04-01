using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.DeleteUser
{
    public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
    {
        private readonly IGenericRepository<User> _userRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteUserCommandHandler(IGenericRepository<User> userRepository, IUnitOfWork unitOfWork)
        {
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
        {
            var user = await _userRepository.GetByIdAsync(request.UserId) ?? throw new Exception($"User with ID {request.UserId} not found.");
            
            _userRepository.Delete(user);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
