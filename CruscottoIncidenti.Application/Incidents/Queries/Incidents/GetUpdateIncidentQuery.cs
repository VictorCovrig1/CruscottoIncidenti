using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries.Incidents
{
    public class GetUpdateIncidentQuery : IRequest<UpdateIncidentViewModel>
    {
        public int Id { get; set; }
    }

    public class GetIncidentToUpdateHandler : IRequestHandler<GetUpdateIncidentQuery, UpdateIncidentViewModel>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetIncidentToUpdateHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<UpdateIncidentViewModel> Handle(GetUpdateIncidentQuery request, CancellationToken cancellationToken)
        {
            return await _context.Incidents
                .Include(x => x.Threat)
                .Include(x => x.Scenario)
                .Include(x => x.Origin)
                .Include(x => x.Ambit)
                .Include(x => x.IncidentType)
                .AsNoTracking()
                .Where(x => x.Id == request.Id && !x.IsDeleted)
                .Select(x => new UpdateIncidentViewModel
                {
                    Id = x.Id,
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
                    IncidentTypeId = x.IncidentTypeId,
                    AmbitId = x.AmbitId,
                    OriginId = x.OriginId,
                    ThreatId = x.ThreatId,
                    ScenarioId = x.ScenarioId,
                    ThirdParty = x.ThirdParty
                }).FirstOrDefaultAsync(cancellationToken);
        }
    }
}
