using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries
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
            return (await _context.Incidents
                .Include(x => x.Threat)
                .Include(x => x.Scenario)
                .Include(x => x.Origin)
                .Include(x => x.Ambit)
                .Include(x => x.IncidentType)
                .AsNoTracking()
                .Where(x => x.Id == request.Id)
                .ToListAsync(cancellationToken))
                .Select(x => new UpdateIncidentViewModel
                {
                    Id = x.Id,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDate = x.OpenDate.ToString("dd/MM/yyyy"),
                    CloseDate = x.CloseDate?.ToString("dd/MM/yyyy"),
                    Type = Enum.TryParse(x.Type, out RequestType type) ? (int)type : 0,
                    ApplicationType = x.ApplicationType,
                    Urgency = Enum.TryParse(x.Urgency, out Urgency urgency) ? (int)urgency : 0,
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
