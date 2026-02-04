using System;
using System.Collections.Generic;

namespace CruscottoIncidenti.Application.Users.ViewModels
{
    public class DetailedUserViewModel
    {
        public int Id { get; set; }

        public string LastModifiedString { get { return LastModified?.ToString("dd/MM/yyyy"); } set { } }

        public DateTime? LastModified { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<string> Roles { get; set; }

        public string RolesString { get { return string.Join(", ", Roles); } set { } }

        public bool ShouldBeDeleted { get; set; }
    }
}
