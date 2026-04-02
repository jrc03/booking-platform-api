using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Reviews.Commands.CreateReview
{
    public class CreateReviewCommandHandler : IRequestHandler<CreateReviewCommand, ReviewResponseDto>
    {

        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<ReviewResponseDto> Handle(CreateReviewCommand request, CancellationToken cancellationToken)
        {
            var newReview = Review.Create(
                bookingId: request.BookingId,
                guestId: request.GuestId,
                propertyId: request.PropertyId,
                rating: request.Rating,
                comment: request.Comment
            );

            _reviewRepository.Add(newReview);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReviewResponseDto
           (
            newReview.Id,
            newReview.BookingId,
            newReview.GuestId,
            newReview.PropertyId,
            newReview.Rating,
            newReview.Comment,
            newReview.CreatedAt
           );


        }
    }
}