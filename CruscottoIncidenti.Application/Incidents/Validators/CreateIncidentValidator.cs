using CruscottoIncidenti.Application.Incidents.Commands.CreateIncident;
using FluentValidation;

namespace CruscottoIncidenti.Application.Incidents.Validators
{
    public class CreateIncidentValidator : AbstractValidator<CreateIncidentCommand>
    {
        public CreateIncidentValidator()
        {
            RuleFor(x => x.CreatorId).NotEmpty().GreaterThan(0);

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

            RuleFor(x => x.ThirdParty)
                .MaximumLength(100).WithMessage("Too long name for Third Party (max. 100)");

            RuleFor(x => x.AmbitId)
                .GreaterThan(0).WithMessage("Ambit ID should be greater than 0");

            RuleFor(x => x.OriginId)
                .GreaterThan(0).WithMessage("Origin ID should be greater than 0");

            RuleFor(x => x.IncidentTypeId)
                .GreaterThan(0).WithMessage("Incident ID should be greater than 0");

            RuleFor(x => x.ThreatId)
                .GreaterThan(0).WithMessage("Threat ID should be greater than 0");

            RuleFor(x => x.ScenarioId)
                .GreaterThan(0).WithMessage("Scenario ID should be greater than 0");
        }
    }
}
