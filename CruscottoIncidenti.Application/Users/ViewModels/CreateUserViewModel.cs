using System.Collections.Generic;
using MediatR;

namespace CruscottoIncidenti.Application.Users.ViewModels
{
    public class CreateUserViewModel : IRequest
    {
        public string Username { get; set; }

        public string Password { get; set; }

        public string ConfirmPassword { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public List<int> Roles { get; set; }
    }
}
