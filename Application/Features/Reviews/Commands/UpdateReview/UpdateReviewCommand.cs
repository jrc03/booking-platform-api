using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Application.Features.Reviews.DTOs;
using MediatR;

namespace Application.Features.Reviews.Commands.UpdateReview
{
   public record UpdateReviewCommand
   (
        Guid Id,
        Guid BookingId,
        Guid GuestId,
        Guid PropertyId,
        int Rating,
        string Comment,
        DateTime CreatedAt
   ) : IRequest<ReviewResponseDto>;
}