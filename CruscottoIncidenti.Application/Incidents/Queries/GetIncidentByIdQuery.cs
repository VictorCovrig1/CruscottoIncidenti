using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.User.Queries;
using CruscottoIncidenti.Application.User.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries
{
    public class GetIncidentByIdQuery : IRequest<IncidentViewModel>
    {
        public int Id { get; set; }
    }

    //public class GetIncidentByIdHandler : IRequestHandler<GetIncidentByIdQuery, IncidentViewModel>
    //{
    //    private readonly ICruscottoIncidentiDbContext _context;

    //    public GetIncidentByIdHandler(ICruscottoIncidentiDbContext context)
    //        => _context = context;

    //    public async Task<IncidentViewModel> Handle(GetIncidentByIdQuery request, CancellationToken cancellationToken)
    //    {
    //        return await _context.Incidents
    //            .AsNoTracking()
    //            .Include("Threats")
    //            .Include("Scenarios")
    //            .Include("Origins")
    //            .Include("")
    //            .Where(x => x.Id == request.Id)
    //            .Select(x => new IncidentViewModel
    //            {
    //                Id = x.Id,
    //                Username = x.UserName,
    //                Email = x.Email,
    //                FullName = x.FullName,
    //                IsEnabled = x.IsEnabled,
    //                Roles = _context.Roles.Select(r => new RoleViewModel
    //                {
    //                    Id = r.Id,
    //                    Name = r.Name,
    //                    IsSelected = x.Roles.Select(ur => ur.Id).Contains(r.Id)
    //                }).ToList()
    //            }).FirstOrDefaultAsync(cancellationToken);
    //    }
    //}
}
