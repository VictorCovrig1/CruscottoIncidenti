using System;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class IncidentRowViewModel
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public string Subsystem { get; set; }

        public DateTime? OpenDateDT { get; set; }

        public string OpenDate { get { return OpenDateDT?.ToString("dd/MM/yyyy"); } set { } }

        public DateTime? CloseDateDT { get; set; }

        public string CloseDate { get { return CloseDateDT?.ToString("dd/MM/yyyy"); } set { } }

        public string Type { get; set; }

        public string Urgency { get; set; }
    }
}
