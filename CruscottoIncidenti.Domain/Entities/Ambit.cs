using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Ambit : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<OriginToAmbit> AmbitToOrigins { get; set; } = new HashSet<OriginToAmbit>();

        public ICollection<AmbitToType> AmbitToTypes { get; set; } = new HashSet<AmbitToType>();

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
