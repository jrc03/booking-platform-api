using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using MediatR;

namespace Application.Features.Reviews.Commands.CreateReview
{
    public record CreateReviewCommand
   (
        Guid BookingId,
        Guid GuestId,
        Guid PropertyId,
        int Rating,
        string Comment
   ) : IRequest<ReviewResponseDto>;

}