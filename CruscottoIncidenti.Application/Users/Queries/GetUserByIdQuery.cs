using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Roles.ViewModels;
using CruscottoIncidenti.Application.User.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.User.Queries
{
    public class GetUserByIdQuery : IRequest<UserViewModel>
    {
        public int Id { get; set; }
    }

    public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUserByIdHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<UserViewModel> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .Where(x => x.Id == request.Id)
                .Select(x => new UserViewModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = _context.Roles.Select(r => new RoleViewModel 
                    { 
                        Id = r.Id,
                        Name = r.Name,
                        IsSelected = x.UserRoles.Select(ur => ur.RoleId).Contains(r.Id)
                    }).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
