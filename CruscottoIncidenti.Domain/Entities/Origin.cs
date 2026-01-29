using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Origin : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<OriginToAmbit> OriginToAmbits { get; set; } = new HashSet<OriginToAmbit>();

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
