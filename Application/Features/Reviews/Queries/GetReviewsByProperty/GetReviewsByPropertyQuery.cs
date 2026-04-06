using Application.Features.Reviews.DTOs;
using MediatR;

namespace Application.Features.Reviews.Queries.GetReviewsByProperty
{
public record GetReviewsByPropertyQuery(Guid PropertyId) : IRequest<PropertyReviewSummaryDto>;
}