using System.Collections.Generic;
using CruscottoIncidenti.Domain.Common;

namespace CruscottoIncidenti.Domain.Entities
{
    public class User : AuditableEntity
    {
        public User() => IsEnabled = true;

        public string UserName { get; set; }
        
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<UserToRole> UserRoles { get; set; } = new HashSet<UserToRole>();
    }
}
