using CruscottoIncidenti.Common;

namespace CruscottoIncidenti.Application.Interfaces
{
    public interface ICurrentUserService
    {
        int UserId { get; }

        Role Role { get; }

        bool IsAuthenticated { get; }

        string UserName { get; }

        string FullName { get; }

        string Email { get; }
    }
}
