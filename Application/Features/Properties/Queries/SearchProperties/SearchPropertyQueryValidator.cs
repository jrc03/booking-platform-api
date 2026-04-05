using FluentValidation;
using System;

namespace Application.Features.Properties.Queries.SearchProperties;

public class SearchPropertyQueryValidator : AbstractValidator<SearchPropertyQuery>
{
    public SearchPropertyQueryValidator()
    {
        RuleFor(x => x.StartDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date)
            .WithMessage("Start Date cannot be in the past.")
            .When(x => x.StartDate.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .WithMessage("End Date must be after Start Date.")
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue);

        RuleFor(x => x.MinCapacity)
            .GreaterThan(0)
            .WithMessage("Capacity must be greater than zero.")
            .When(x => x.MinCapacity.HasValue);

        RuleFor(x => x.MaxPrice)
            .GreaterThan(0)
            .WithMessage("Price must be greater than zero.")
            .When(x => x.MaxPrice.HasValue);

        RuleFor(x => x.PageNumber)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page Number must be at least 1.");

        RuleFor(x => x.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Page Size must be at least 1.")
            .LessThanOrEqualTo(100)
            .WithMessage("Page Size cannot exceed 100.");
    }
}