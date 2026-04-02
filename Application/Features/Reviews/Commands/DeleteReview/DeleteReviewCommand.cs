using MediatR;
using System;

namespace Application.Features.Reviews.Commands.DeleteReview
{
    public record DeleteReviewCommand(Guid ReviewId, Guid GuestId) : IRequest<bool>;
}
