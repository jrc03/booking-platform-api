using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Reviews.Commands.UpdateReview
{
    public class UpdateReviewCommandHandler : IRequestHandler<UpdateReviewCommand, ReviewResponseDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateReviewCommandHandler(IReviewRepository reviewRepository, IUnitOfWork unitOfWork)
        {
            _reviewRepository = reviewRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<ReviewResponseDto> Handle(UpdateReviewCommand request, CancellationToken cancellationToken)
        {
            var review = await _reviewRepository.GetByIdAsync(request.Id) ?? throw new Exception("Review not found");

            review.UpdateReview(
                request.Rating,
                request.Comment
            );

            _reviewRepository.Update(review);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new ReviewResponseDto
          (
           review.Id,
           review.BookingId,
           review.GuestId,
           review.PropertyId,
           review.Rating,
           review.Comment,
           review.CreatedAt
          );


        }
    }
}