using FluentValidation;
using NatoursApi.Controllers.V1.Dtos;

namespace NatoursApi.Controllers.V1.Validators;

internal sealed class CreateTourValidator : AbstractValidator<CreateTourDto>
{
    public CreateTourValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(10, 40).WithMessage("Name must be between 10 and 40 characters");

        RuleFor(x => x.Duration).GreaterThan(0);

        RuleFor(x => x.MaxGroupSize).GreaterThan(0).LessThan(100);

        RuleFor(x => x.Difficulty).NotEmpty().WithMessage("Difficulty is required")
            .Must(x => BeOneOf(x, "easy", "medium", "difficult"))
            .WithMessage("Difficulty must be one of: easy, medium or difficult");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .NotNull().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0);

        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("Summary is required")
            .NotNull().WithMessage("Summary is required");
    }

    private static bool BeOneOf(string value, params string[] allowed)
    {
        return allowed.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}