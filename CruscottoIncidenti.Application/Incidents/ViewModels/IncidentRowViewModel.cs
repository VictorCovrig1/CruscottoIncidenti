namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class IncidentRowViewModel
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public string Subsystem { get; set; }

        public string OpenDate { get; set; }

        public string CloseDate { get; set; }

        public string Type { get; set; }

        public string Urgency { get; set; }
    }
}
