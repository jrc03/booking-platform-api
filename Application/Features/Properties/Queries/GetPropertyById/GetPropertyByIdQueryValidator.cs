using FluentValidation;

namespace Application.Features.Properties.Queries.GetPropertyById;

public class GetPropertyByIdQueryValidator : AbstractValidator<GetPropertyByIdQuery>
{
    public GetPropertyByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Property ID is required.");
    }
}