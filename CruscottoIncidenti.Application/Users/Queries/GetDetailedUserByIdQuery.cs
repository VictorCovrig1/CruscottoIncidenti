using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Users.ViewModels;
using MediatR;
using System.Linq;

namespace CruscottoIncidenti.Application.User.Queries
{
    public class GetDetailedUserByIdQuery : IRequest<DetailedUserViewModel>
    {
        public int Id { get; set; }
    }

    public class GetDetailedUserByIdHandler : IRequestHandler<GetDetailedUserByIdQuery, DetailedUserViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetDetailedUserByIdHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<DetailedUserViewModel> Handle(GetDetailedUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .Where(x => x.Id == request.Id)
                .Select(x => new DetailedUserViewModel
                {
                    Id = x.Id,
                    LastModified = x.LastModified,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = x.UserRoles.Select(r => r.Role.Name).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
