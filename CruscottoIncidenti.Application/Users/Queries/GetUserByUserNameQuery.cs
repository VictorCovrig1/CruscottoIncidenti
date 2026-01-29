using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Roles.ViewModels;
using CruscottoIncidenti.Application.User.ViewModels;
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
            string encrypted = string.Empty;

            using (SHA256 hash = SHA256.Create())
            {
                encrypted = string.Concat(hash
                    .ComputeHash(Encoding.UTF8.GetBytes(request.Password))
                    .Select(item => item.ToString("x2")));
            }

            return await _context.Users
                .AsNoTracking()
                .Where(x => x.UserName.ToLower() == request.UserName.ToLower() && 
                    x.IsEnabled && x.Password == encrypted)
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
