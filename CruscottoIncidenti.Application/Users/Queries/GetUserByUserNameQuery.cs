using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Utils;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Roles.ViewModels;
using CruscottoIncidenti.Application.Users.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.User.Queries
{
    public class GetUserByUserNameQuery : IRequest<UserViewModel>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class GetUserByUserNameHandler : IRequestHandler<GetUserByUserNameQuery, UserViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUserByUserNameHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<UserViewModel> Handle(GetUserByUserNameQuery request, CancellationToken cancellationToken)
        {
            string encryptedPassword = PasswordHelper.EncryptPassword(request.Password);

            return await _context.Users
                .AsNoTracking()
                .Where(x => x.UserName.ToLower() == request.UserName.ToLower() && x.IsEnabled && x.Password == encryptedPassword)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = x.UserRoles.Select(r => new RoleViewModel { Id = r.RoleId, Name = r.Role.Name }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
