using System;
using System.Globalization;
using CruscottoIncidenti.Common;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.ViewModels
{
    public class UpdateIncidentViewModel : IRequest
    {
        public int Id { get; set; }

        public string RequestNr { get; set; }

        public string Subsystem { get; set; }

        public DateTime? OpenDate { get; set; }

        public string OpenDateString
        {
            get
            {
                return OpenDate != null ? OpenDate?.ToString("dd/MM/yyyy") : null;
            }
            set
            {
                OpenDate = DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime openDate)
                    ? (DateTime?)openDate
                    : null;
            }
        }

        public DateTime? CloseDate { get; set; }

        public string CloseDateString
        {
            get
            {
                return CloseDate != null ? CloseDate?.ToString("dd/MM/yyyy") : null;
            }
            set
            {
                CloseDate = DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out DateTime closeDate)
                    ? (DateTime?)closeDate
                    : null;
            }
        }

        public string Type { get; set; }

        public int TypeInt
        {
            get
            {
                return Enum.TryParse(Type, out RequestType type) ? (int)type : 0;
            }
            set
            {
                Type = Enum.GetName(typeof(RequestType), value);
            }
        }

        public string ApplicationType { get; set; }

        public string Urgency { get; set; }

        public int UrgencyInt
        {
            get
            {
                return Enum.TryParse(Urgency, out Urgency urgency) ? (int)urgency : 0;
            }
            set
            {
                Urgency = Enum.GetName(typeof(Urgency), value);
            }
        }

        public string SubCause { get; set; }

        public string ProblemSummary { get; set; }

        public string ProblemDescription { get; set; }

        public string Solution { get; set; }

        public int? IncidentTypeId { get; set; }

        public int? AmbitId { get; set; }

        public int OriginId { get; set; }

        public int ThreatId { get; set; }

        public int ScenarioId { get; set; }

        public string ThirdParty { get; set; }
    }
}
