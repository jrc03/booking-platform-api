using FluentValidation;

namespace Application.Features.Reviews.Commands.DeleteReview
{
    public class DeleteReviewCommandValidator : AbstractValidator<DeleteReviewCommand>
    {
        public DeleteReviewCommandValidator()
        {
            RuleFor(x => x.ReviewId)
                .NotEmpty().WithMessage("Review ID is required.");

            RuleFor(x => x.GuestId)
                .NotEmpty().WithMessage("Guest ID is required.");
        }
    }
}
