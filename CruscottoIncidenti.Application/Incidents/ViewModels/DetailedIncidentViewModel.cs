using System;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class DetailedIncidentViewModel
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public DateTime? LastModified { get; set; }

        public string LastModifiedString { get { return LastModified?.ToString("dd/MM/yyyy"); } set { } }

        public string Subsystem { get; set; }

        public DateTime? OpenDate { get; set; }

        public string OpenDateString { get { return OpenDate?.ToString("dd/MM/yyyy"); } set { } }

        public DateTime? CloseDate { get; set; }

        public string CloseDateString { get { return CloseDate?.ToString("dd/MM/yyyy"); } set { } }

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
