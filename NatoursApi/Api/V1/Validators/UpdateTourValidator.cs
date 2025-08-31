using FluentValidation;
using NatoursApi.Api.V1.Dtos;

namespace NatoursApi.Api.V1.Validators;

internal sealed class UpdateTourValidator : AbstractValidator<UpdateTourDto>
{
    public UpdateTourValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required")
            .Length(10, 40).WithMessage("Name must be between 10 and 40 characters");


        RuleFor(x => x.Duration)
            .GreaterThan(0).WithMessage("Duration must be greater than 0");

        RuleFor(x => x.MaxGroupSize)
            .GreaterThan(0).WithMessage("MaxGroupSize must be greater than 0")
            .LessThan(100).WithMessage("MaxGroupSize must be less than 100");

        RuleFor(x => x.Difficulty)
            .NotEmpty().WithMessage("Difficulty is required")
            .Must(x => BeOneOf(x, "easy", "medium", "difficult"))
            .WithMessage("Difficulty must be one of: easy, medium or difficult");

        RuleFor(x => x.Price)
            .NotEmpty().WithMessage("Price is required")
            .GreaterThanOrEqualTo(0).WithMessage("Price must be greater than or equal to 0");

        RuleFor(x => x.PriceDiscount)
            .Must((tour, priceDiscount) => priceDiscount is null || priceDiscount < tour.Price)
            .WithMessage("PriceDiscount must be less than the actual Price")
            .When(x => x.PriceDiscount.HasValue);

        RuleFor(x => x.Summary)
            .NotEmpty().WithMessage("Summary is required");

        RuleFor(x => x.Description)
            .MaximumLength(2000).WithMessage("Description must not exceed 2000 characters")
            .When(x => x.Description != null);
    }

    private static bool BeOneOf(string value, params string[] allowed)
    {
        return allowed.Contains(value.Trim(), StringComparer.OrdinalIgnoreCase);
    }
}