using System.Collections.Generic;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class ImportIncidentsViewModel : IRequest<(string, List<string>)>
    {
        public List<ImportIncidentViewModel> Incidents { get; set; }
    }

    public class ImportIncidentViewModel
    {
        public string RequestNr { get; set; }

        public string Subsystem { get; set; }

        public string OpenDate { get; set; }

        public string CloseDate { get; set; }

        public string Type { get; set; }

        public string ApplicationType { get; set; }

        public string Urgency { get; set; }

        public string SubCause { get; set; }

        public string ProblemSummary { get; set; }

        public string ProblemDescription { get; set; }

        public string Solution { get; set; }

        public string IncidentType { get; set; }

        public string Ambit { get; set; }

        public string Origin { get; set; }

        public string Threat { get; set; }

        public string Scenario { get; set; }

        public string ThirdParty { get; set; }
    }
}
