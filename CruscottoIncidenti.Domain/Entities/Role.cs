using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class Role : BaseEntity
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<UserToRole> RoleUsers { get; set; } = new HashSet<UserToRole>();
    }
}
