using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Origin : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<Ambit> Ambits { get; set; } = new HashSet<Ambit>();

        public ICollection<Incident> Incidents { get; set; } = new HashSet<Incident>();
    }
}
