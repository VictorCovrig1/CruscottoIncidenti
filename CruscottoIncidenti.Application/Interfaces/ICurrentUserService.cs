using System.Collections.Generic;
using CruscottoIncidenti.Common;

namespace CruscottoIncidenti.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        ICollection<Role> Roles { get; }

        bool IsAuthenticated { get; }

        string UserName { get; }

        string FullName { get; }

        string Email { get; }
    }
}
