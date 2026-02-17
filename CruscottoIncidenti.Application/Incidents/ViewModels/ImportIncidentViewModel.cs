using System.Collections.Generic;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class ImportIncidentsViewModel : IRequest<(string, List<string>)>
    {
        public List<CreateIncidentViewModel> Incidents { get; set; }
    }
}
