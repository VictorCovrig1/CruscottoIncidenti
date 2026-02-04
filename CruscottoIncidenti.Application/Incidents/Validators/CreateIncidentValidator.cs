using CruscottoIncidenti.Application.Incidents.ViewModels;
using FluentValidation;

namespace CruscottoIncidenti.Application.Incidents.Validators
{
    public class CreateIncidentValidator : AbstractValidator<CreateIncidentViewModel>
    {
        public CreateIncidentValidator()
        {
            RuleFor(x => x.RequestNr)
                .NotEmpty().WithMessage("Request Number can't be empty");

            RuleFor(x => x.Subsystem)
                .NotEmpty().WithMessage("Subsystem can't be empty")
                .Length(2).WithMessage("Subsystem must have 2 characters");

            RuleFor(x => x.Type)
                .NotEmpty().WithMessage("Request Type can't be empty")
                .LessThan(6).WithMessage("Request Type should be less than 6");

            RuleFor(x => x.Urgency)
                .NotEmpty().WithMessage("Urgency can't be empty")
                .LessThan(6).WithMessage("Urgency should be less than 6");

            RuleFor(x => x.ApplicationType)
                .NotEmpty().WithMessage("Application Type can't be empty")
                .MaximumLength(50).WithMessage("Too long name for Application Type (max. 50)");

            RuleFor(x => x.SubCause)
                .NotEmpty().WithMessage("Subcause can't be empty")
                .MaximumLength(100).WithMessage("Too long name for Subcause (max. 100)");

            RuleFor(x => x.ProblemSummary)
                .NotEmpty().WithMessage("Problem Summary can't be empty")
                .MaximumLength(500).WithMessage("Too long name for Problem Summary (max. 500)");

            RuleFor(x => x.ProblemDescription)
                .NotEmpty().WithMessage("Problem Description can't be empty");

            RuleFor(x => x.OriginId)
                .NotEmpty().WithMessage("Origin can't be empty");

            RuleFor(x => x.AmbitId)
                .NotEmpty().WithMessage("Ambit can't be empty");

            RuleFor(x => x.IncidentTypeId)
                .NotEmpty().WithMessage("Incident Type can't be empty");

            RuleFor(x => x.ThreatId)
                .NotEmpty().WithMessage("Threat can't be empty");

            RuleFor(x => x.ScenarioId)
                .NotEmpty().WithMessage("Scenario can't be empty");

            RuleFor(x => x.ThirdParty)
                .NotEmpty().WithMessage("Third Party can't be empty")
                .MaximumLength(100).WithMessage("Too long name for Third Party (max. 100)");
        }
    }
}
