using System.Collections.Generic;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class ImportIncidentsViewModel : IRequest
    {
        public List<CreateIncidentViewModel> Incidents { get; set; }
    }
}
