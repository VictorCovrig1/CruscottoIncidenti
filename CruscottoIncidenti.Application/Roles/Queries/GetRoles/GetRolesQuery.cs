using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.Roles.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.Roles.Queries.GetRoles
{
    public class GetRolesQuery : IRequest<ICollection<RoleViewModel>>
    {

    }

    public class GetRolesHandler : IRequestHandler<GetRolesQuery, ICollection<RoleViewModel>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetRolesHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<ICollection<RoleViewModel>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            return await _context.Roles.Select(role => new RoleViewModel 
            {
                Id = role.Id, 
                Name = role.Name 
            }).ToListAsync();
        }
    }
}
