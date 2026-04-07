using Application.Interfaces.Auth;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandHandler : IRequestHandler<DeleteReviewCommand, bool>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.ReviewId) ?? throw new Exception($"Review with ID {request.ReviewId} not found.");

            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");
            
            if (review.GuestId != guestId)
                throw new UnauthorizedAccessException("You are not authorized to delete this review.");

            _reviewRepository.Delete(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}

