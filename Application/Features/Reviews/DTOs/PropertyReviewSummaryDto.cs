namespace Application.Features.Reviews.DTOs
{
    public record PropertyReviewSummaryDto(
        decimal AverageRating,
        int TotalReviews,
        IEnumerable<ReviewResponseDto> Reviews
    );
}