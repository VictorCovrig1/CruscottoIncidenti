using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.User.ViewModels;
using MediatR;
using System.Linq;

namespace CruscottoIncidenti.Application.User.Queries.GetDetailedUserById
{
    public class GetDetailedUserByIdQuery : IRequest<DetailedUserViewModel>
    {
        public int Id { get; set; }
    }

    public class GetDetailedUserByIdQueryHandler : IRequestHandler<GetDetailedUserByIdQuery, DetailedUserViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetDetailedUserByIdQueryHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<DetailedUserViewModel> Handle(GetDetailedUserByIdQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .Include(x => x.Roles)
                .Where(x => x.Id == request.Id)
                .Select(x => new DetailedUserViewModel
                {
                    Id = x.Id,
                    Created = x.Created,
                    CreatedBy = x.CreatedBy,
                    LastModified = x.LastModified,
                    LastModifiedBy = x.LastModifiedBy,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = x.Roles.Select(r => r.Name).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
