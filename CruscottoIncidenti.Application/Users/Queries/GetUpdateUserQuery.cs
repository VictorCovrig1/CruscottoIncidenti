using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Users.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.User.Queries
{
    public class GetUpdateUserQuery : IRequest<UpdateUserViewModel>
    {
        public int Id { get; set; }
    }

    public class GetUserToUpdateHandler : IRequestHandler<GetUpdateUserQuery, UpdateUserViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUserToUpdateHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<UpdateUserViewModel> Handle(GetUpdateUserQuery request, CancellationToken cancellationToken)
        {
            return await _context.Users
                .AsNoTracking()
                .Include(x => x.UserRoles)
                .Where(x => x.Id == request.Id)
                .Select(x => new UpdateUserViewModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    FullName = x.FullName,
                    IsEnabled = x.IsEnabled,
                    Roles = x.UserRoles.Select(r => r.RoleId).ToList()
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
