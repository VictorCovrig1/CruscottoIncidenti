using System;
using System.Linq;
using System.Security.Claims;
using System.Web;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;

namespace CruscottoIncidenti.Infrastructure.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsIdentity _userIdentity;

        public CurrentUserService(HttpContextBase httpContext)
        {
            _userIdentity = (ClaimsIdentity)httpContext.User.Identity;

            bool isValidUserId = int.TryParse(_userIdentity.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value, out int userId);
            if (isValidUserId)
                UserId = userId;

            bool isValidRole = Enum.TryParse(_userIdentity.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value, out Role role);
            if (isValidRole)
                Role = role;
        }

        public int UserId { get; }

        public Role Role { get; }

        public bool IsAuthenticated { get; }

        public string UserName => _userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

        public string Email => _userIdentity.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

        public string FullName => _userIdentity.Claims.FirstOrDefault(c => c.Type == "FullName")?.Value;
    }
}
