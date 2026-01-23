using System;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class IncidentRowViewModel
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public string Subsystem { get; set; }

        public DateTime OpenDate { get; set; }

        public DateTime? CloseDate { get; set; }

        public string Type { get; set; }

        public int? Urgency { get; set; }
    }
}
