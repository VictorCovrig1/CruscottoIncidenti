using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.Users.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.User.Queries
{
    public class GetUsersGridQuery : IRequest<List<UserRowViewModel>>
    {
        public DataTablesParameters Parameters { get; set; }
    }

    public class GetUsersGridHandler : IRequestHandler<GetUsersGridQuery, List<UserRowViewModel>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUsersGridHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<List<UserRowViewModel>> Handle(GetUsersGridQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .OrderBy(request.Parameters)
                .Search(request.Parameters)
                .Page(request.Parameters)
                .Select(x => new UserRowViewModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                })
                .ToListAsync(cancellationToken);
        }
    }
}
