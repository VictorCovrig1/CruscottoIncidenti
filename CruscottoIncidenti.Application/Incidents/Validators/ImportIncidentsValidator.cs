using CruscottoIncidenti.Application.Incidents.ViewModels;
using FluentValidation;

namespace CruscottoIncidenti.Application.Incidents.Validators
{
    public class ImportIncidentsValidator : AbstractValidator<ImportIncidentsViewModel>
    {
        public ImportIncidentsValidator()
        {
            RuleForEach(x => x.Incidents).SetValidator(new CreateIncidentValidator());
        }
    }
}
