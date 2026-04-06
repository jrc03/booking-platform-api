using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using Domain.Entities;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Reviews.Queries.GetReviewsByProperty
{
    public class GetReviewsByPropertyQueryHandler : IRequestHandler<GetReviewsByPropertyQuery, PropertyReviewSummaryDto>
    {
        private readonly IReviewRepository _reviewRepository;
    

        public GetReviewsByPropertyQueryHandler(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }

        public async Task<PropertyReviewSummaryDto> Handle(GetReviewsByPropertyQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.GetReviewsByPropertyAsync(request.PropertyId);

            var reviewDtos = reviews.Select(r => new ReviewResponseDto(
                  r.Id, r.BookingId, r.GuestId, r.PropertyId,
                  r.Rating, r.Comment, r.CreatedAt
              )).ToList();

            int totalReviews = reviewDtos.Count();
            decimal averageRating = totalReviews > 0 ? (decimal)reviewDtos.Average(r => r.Rating) : 0;
            
            return new PropertyReviewSummaryDto(averageRating, totalReviews, reviewDtos);

        }
    }
}