using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Ambit : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Origin> Origins { get; set; } = new HashSet<Origin>();

        public ICollection<IncidentType> IncidentTypes { get; set; } = new HashSet<IncidentType>();

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
