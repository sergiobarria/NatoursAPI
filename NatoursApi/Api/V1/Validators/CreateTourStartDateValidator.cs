using FluentValidation;
using NatoursApi.Api.V1.Dtos;

namespace NatoursApi.Api.V1.Validators;

internal sealed class CreateTourStartDateValidator : AbstractValidator<CreateTourStartDateDto>
{
    public CreateTourStartDateValidator()
    {
        RuleFor(x => x.Date)
            .NotEmpty().WithMessage("Date is required")
            .Must(BeInFuture).WithMessage("Date must be in the future");
    }

    private static bool BeInFuture(DateTime date)
    {
        return date > DateTime.UtcNow.AddDays(-1);
    }
}