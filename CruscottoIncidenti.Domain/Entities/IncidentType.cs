using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class IncidentType : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<AmbitToType> TypeToAmbits { get; set; } = new HashSet<AmbitToType>();

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
