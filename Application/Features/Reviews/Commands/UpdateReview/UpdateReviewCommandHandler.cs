using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using Domain.Interfaces;
using MediatR;
using Application.Interfaces.Auth;

namespace Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResponseDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ReviewResponseDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id) ?? throw new Exception("Review not found");

            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");
            
            if (review.GuestId != guestId)
                throw new UnauthorizedAccessException("You are not authorized to update this review.");

            review.UpdateReview(
                request.Rating,
                request.Comment
            );

            _reviewRepository.Update(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

                        var guest = await _userRepository.GetByIdAsync(guestId);

            return new ReviewResponseDto
          (
           review.Id,
           review.BookingId,
           review.GuestId,
                     guest?.FirstName ?? "Guest",
           review.PropertyId,
           review.Rating,
           review.Comment,
           review.CreatedAt
          );


        }
    }
}
