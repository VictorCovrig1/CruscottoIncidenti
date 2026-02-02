using System.Collections.Generic;
using CruscottoIncidenti.Application.Roles.ViewModels;

namespace CruscottoIncidenti.Application.Users.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public string FullName { get; set; }

        public bool IsEnabled { get; set; }

        public ICollection<RoleViewModel> Roles { get; set; }
    }
}
