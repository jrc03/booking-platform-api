using Application.Features.Reviews.DTOs;
using Domain.Interfaces;
using MediatR;

namespace Application.Features.Reviews.Queries.GetReviewsByProperty
{
    public class GetReviewsByPropertyQueryHandler : IRequestHandler<GetReviewsByPropertyQuery, PropertyReviewSummaryDto>
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IUserRepository _userRepository;


        public GetReviewsByPropertyQueryHandler(IReviewRepository reviewRepository, IUserRepository userRepository)
        {
            _reviewRepository = reviewRepository;
            _userRepository = userRepository;
        }

        public async Task<PropertyReviewSummaryDto> Handle(GetReviewsByPropertyQuery request, CancellationToken cancellationToken)
        {
            var reviews = await _reviewRepository.GetReviewsByPropertyAsync(request.PropertyId);

            var guestIds = reviews.Select(review => review.GuestId).Distinct().ToList();
            var guests = new Dictionary<Guid, string>();

            foreach (var guestId in guestIds)
            {
                var guest = await _userRepository.GetByIdAsync(guestId);
                guests[guestId] = guest?.FirstName ?? "Guest";
            }

            var reviewDtos = reviews.Select(review => new ReviewResponseDto(
                review.Id,
                review.BookingId,
                review.GuestId,
                guests.TryGetValue(review.GuestId, out var guestFirstName) ? guestFirstName : "Guest",
                review.PropertyId,
                review.Rating,
                review.Comment,
                review.CreatedAt
            )).ToList();

            int totalReviews = reviewDtos.Count;
            decimal averageRating = totalReviews > 0 ? (decimal)reviewDtos.Average(r => r.Rating) : 0;

            return new PropertyReviewSummaryDto(averageRating, totalReviews, reviewDtos);

        }
    }
}