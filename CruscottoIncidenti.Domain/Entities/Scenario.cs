using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Scenario : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
