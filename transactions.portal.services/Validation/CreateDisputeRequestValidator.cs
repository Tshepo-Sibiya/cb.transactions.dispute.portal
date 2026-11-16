using FluentValidation;
using transactions.portal.services.Dtos;

namespace transactions.portal.services.Validation;

public class CreateDisputeRequestValidator : AbstractValidator<CreateDisputeRequest>
{
    private static readonly string[] AllowedReasons =
    {
        "Unauthorized charge",
        "Duplicate charge",
        "Incorrect amount",
        "Goods not received",
        "Other"
    };

    public CreateDisputeRequestValidator()
    {
        RuleFor(x => x.Reason)
            .NotEmpty().WithMessage("Reason is required.")
            .Must(reason => AllowedReasons.Contains(reason))
            .WithMessage("Reason must be one of: Unauthorized charge, Duplicate charge, Incorrect amount, Goods not received, Other.");

        RuleFor(x => x.Description)
            .NotEmpty().WithMessage("Description is required.")
            .Length(10, 500).WithMessage("Description must be between 10 and 500 characters.");
    }
}

