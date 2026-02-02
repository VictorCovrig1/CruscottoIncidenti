using MediatR;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class CreateIncidentViewModel : IRequest
    {
        public string Subsystem { get; set; }

        public int Type { get; set; }

        public string ApplicationType { get; set; }

        public int? Urgency { get; set; }

        public string SubCause { get; set; }

        public string ProblemSumary { get; set; }

        public string ProblemDescription { get; set; }

        public int? IncidentTypeId { get; set; }

        public int? AmbitId { get; set; }

        public int? OriginId { get; set; }

        public int? ThreatId { get; set; }

        public int? ScenarioId { get; set; }

        public string ThirdParty { get; set; }
    }
}
