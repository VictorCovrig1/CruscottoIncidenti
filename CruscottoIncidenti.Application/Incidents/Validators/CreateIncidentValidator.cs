using System;
using CruscottoIncidenti.Application.Incidents.Commands.CreateIncident;
using CruscottoIncidenti.Common;
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
                .IsEnumName(typeof(RequestType));

            RuleFor(x => x.Urgency).Must(x => Enum.IsDefined(typeof(Urgency), x));

            RuleFor(x => x.ApplicationType)
                .MaximumLength(50).WithMessage("Too long name for Application Type (max. 50)");

            RuleFor(x => x.SubCause)
                .MaximumLength(100).WithMessage("Too long name for Subcause (max. 100)");

            RuleFor(x => x.ProblemSumary)
                .NotEmpty().WithMessage("Problem Summary can't be empty")
                .MaximumLength(500).WithMessage("Too long name for Problem Summary (max. 500)");

            RuleFor(x => x.ThirdParty)
                .MaximumLength(100).WithMessage("Too long name for Third Party (max. 100)");

            RuleFor(x => x.AmbitId).GreaterThan(0);

            RuleFor(x => x.OriginId).GreaterThan(0);

            RuleFor(x => x.IncidentTypeId).GreaterThan(0);

            RuleFor(x => x.ThreatId).GreaterThan(0);

            RuleFor(x => x.ScenarioId).GreaterThan(0);
        }
    }
}
