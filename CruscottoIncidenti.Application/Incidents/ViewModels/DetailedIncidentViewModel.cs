namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class DetailedIncidentViewModel
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public string LastModified { get; set; }

        public string Subsystem { get; set; }

        public string OpenDate { get; set; }

        public string CloseDate { get; set; }

        public string Type { get; set; }

        public string ApplicationType { get; set; }

        public string Urgency { get; set; }

        public string SubCause { get; set; }

        public string ProblemSumary { get; set; }

        public string ProblemDescription { get; set; }

        public string Solution { get; set; }

        public int? IncidentTypeId { get; set; }

        public int? AmbitId { get; set; }

        public string AmbitName { get; set; }

        public int? OriginId { get; set; }

        public string OriginName { get; set; }

        public int? ThreatId { get; set; }

        public int? ScenarioId { get; set; }

        public string ThirdParty { get; set; }

        public bool ShouldBeDeleted { get; set; }
    }
}
