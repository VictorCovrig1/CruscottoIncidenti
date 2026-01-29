using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.User.ViewModels;
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
            var roles = _context.Users.Where(x => x.Id == request.Id).Include(x => x.UserRoles).Select(x => x.UserRoles.Select(y => y.RoleId)).ToList();

            var result = await _context.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .Where(x => x.Id == request.Id)
                .Select(x => new DetailedUserViewModel
                {
                    Id = x.Id,
                    Created = x.Created == null ? string.Empty : x.Created.ToString(),
                    CreatedBy = x.CreatedBy,
                    LastModified = x.LastModified == null ? string.Empty : x.LastModified.ToString(),
                    LastModifiedBy = x.LastModifiedBy,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = x.UserRoles.Select(r => r.RoleId).ToList()
                }).FirstOrDefaultAsync(cancellationToken);

            return result;
        }
    }
}
