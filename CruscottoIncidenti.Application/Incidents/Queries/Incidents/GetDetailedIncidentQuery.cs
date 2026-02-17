using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Data.Entity;

namespace CruscottoIncidenti.Application.Incidents.Queries.Incidents
{
    public class GetDetailedIncidentQuery : IRequest<DetailedIncidentViewModel>
    {
        public int Id { get; set; }
    }

    public class GetDetailedIncidentHandler : IRequestHandler<GetDetailedIncidentQuery, DetailedIncidentViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetDetailedIncidentHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<DetailedIncidentViewModel> Handle(GetDetailedIncidentQuery request, CancellationToken cancellationToken)
        {
            return await _context.Incidents
                .Include(x => x.Threat)
                .Include(x => x.Scenario)
                .Include(x => x.Origin)
                .Include(x => x.Ambit)
                .Include(x => x.IncidentType)
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new DetailedIncidentViewModel
                {
                    Id = x.Id,
                    LastModified = x.LastModified,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate,
                    Type = x.Type,
                    ApplicationType = x.ApplicationType,
                    Urgency = x.Urgency,
                    SubCause = x.SubCause,
                    ProblemSummary = x.ProblemSumary,
                    ProblemDescription = x.ProblemDescription,
                    Solution = x.Solution,
                    IncidentType = x.IncidentType.Name,
                    Ambit = x.Ambit.Name,
                    Origin = x.Origin.Name,
                    Threat = x.Threat.Name,
                    Scenario = x.Scenario.Name,
                    ThirdParty = x.ThirdParty
                }).FirstOrDefaultAsync(cancellationToken);
            
        }
    }
}