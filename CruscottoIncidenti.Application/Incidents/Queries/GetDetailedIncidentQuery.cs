using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;
using System.Linq;
using System.Data.Entity;

namespace CruscottoIncidenti.Application.Incidents.Queries
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
            return (await _context.Incidents
                .Include(x => x.Threat)
                .Include(x => x.Scenario)
                .Include(x => x.Origin)
                .Include(x => x.Ambit)
                .Include(x => x.IncidentType)
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ToListAsync(cancellationToken))
                .Select(x => new DetailedIncidentViewModel
                {
                    Id = x.Id,
                    LastModified = x.LastModified.HasValue ?
                        x.LastModified.Value.ToString("dd/MM/yyyy") :
                        null,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDate = x.OpenDate.ToString("dd/MM/yyyy"),
                    CloseDate = x.CloseDate?.ToString("dd/MM/yyyy"),
                    Type = x.Type,
                    ApplicationType = x.ApplicationType,
                    Urgency = x.Urgency,
                    SubCause = x.SubCause,
                    ProblemSumary = x.ProblemSumary,
                    ProblemDescription = x.ProblemDescription,
                    Solution = x.Solution,
                    IncidentTypeId = x.IncidentTypeId,
                    AmbitId = x.AmbitId,
                    OriginId = x.OriginId,
                    ThreatId = x.ThreatId,
                    ScenarioId = x.ScenarioId,
                    ThirdParty = x.ThirdParty
                }).FirstOrDefault();
            
        }
    }
}