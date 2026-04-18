using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

using Application.Interfaces.Auth;

namespace Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponseDto>
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IBookingRepository bookingRepository, IUserRepository userRepository, IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _reviewRepository = reviewRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }

        public async Task<ReviewResponseDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var guestId = _currentUserService.UserId ?? throw new UnauthorizedAccessException("Invalid user.");

            var booking = await _bookingRepository.GetByIdAsync(request.BookingId) ?? throw new ArgumentException("Booking not found.");
           
            if (booking.GuestId != guestId)
                throw new UnauthorizedAccessException("You are not the guest of this booking.");
            if (booking.Status != Domain.Enums.BookingStatus.Completed)
                throw new InvalidOperationException("You can only review a property after the booking is completed.");

            var newReview = Review.Create(
                bookingId: request.BookingId,
                guestId: guestId,
                propertyId: request.PropertyId,
                rating: request.Rating,
                comment: request.Comment
            );

            _reviewRepository.Add(newReview);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var guest = await _userRepository.GetByIdAsync(guestId);

            return new ReviewResponseDto
           (
            newReview.Id,
            newReview.BookingId,
            newReview.GuestId,
            guest?.FirstName ?? "Guest",
            newReview.PropertyId,
            newReview.Rating,
            newReview.Comment,
            newReview.CreatedAt
           );


        }
    }
}
