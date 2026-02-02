using System.Collections.Generic;
using MediatR;

namespace CruscottoIncidenti.Application.Users.ViewModels
{
    public class UpdateUserViewModel : IRequest
    {
        public int Id { get; set; }

        public string FullName { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public bool IsPasswordEnabled { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public IList<int> Roles { get; set; }

        public bool IsEnabled { get; set; }
    }
}
