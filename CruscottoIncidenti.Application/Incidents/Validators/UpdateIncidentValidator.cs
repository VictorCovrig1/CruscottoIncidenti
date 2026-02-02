using CruscottoIncidenti.Application.Incidents.ViewModels;
using FluentValidation;

namespace CruscottoIncidenti.Application.Incidents.Validators
{
    public class UpdateIncidentValidator : AbstractValidator<UpdateIncidentViewModel>
    {
        public UpdateIncidentValidator()
        {
            RuleFor(x => x.Subsystem).Length(2).WithMessage("Subsystem must have 2 characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Request Type can't be empty")
                .GreaterThan(0).WithMessage("Request Type should be greater than 0")
                .LessThan(6).WithMessage("Request Type should be less than 6");

            RuleFor(x => x.Urgency)
                .GreaterThan(0).WithMessage("Urgency should be greater than 0")
                .LessThan(6).WithMessage("Urgency should be less than 6");

            RuleFor(x => x.ApplicationType)
                .MaximumLength(50).WithMessage("Too long name for Application Type (max. 50)");

            RuleFor(x => x.SubCause)
                .MaximumLength(100).WithMessage("Too long name for Subcause (max. 100)");

            RuleFor(x => x.ProblemSumary)
                .NotEmpty().WithMessage("Problem Summary can't be empty")
                .MaximumLength(500).WithMessage("Too long name for Problem Summary (max. 500)");

            RuleFor(x => x.ProblemDescription)
                .NotEmpty().WithMessage("Problem Description can't be empty");

            RuleFor(x => x.ThirdParty)
                .MaximumLength(100).WithMessage("Too long name for Third Party (max. 100)");
        }
    }
}
