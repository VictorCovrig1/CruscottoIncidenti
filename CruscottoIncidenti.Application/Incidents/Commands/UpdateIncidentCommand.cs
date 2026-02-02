using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Domain.Entities;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class UpdateIncidentHandler : IRequestHandler<UpdateIncidentViewModel, Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateIncidentHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(UpdateIncidentViewModel request, CancellationToken cancellationToken)
        {
            Scenario scenario = null;
            if (request.ScenarioId != null)
            {
                scenario = await _context.Scenarios
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ScenarioId);
                if (scenario == null)
                    throw new CustomException($"Scenario ({request.ScenarioId}) not found");
            }

            Threat threat = null;
            if (request.ThreatId != null)
            {
                threat = await _context.Threats
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.ScenarioId);
                if (threat == null)
                    throw new CustomException($"Threat ({request.ThreatId}) not found");
            }

            Origin origin = null;
            if(request.OriginId != null)
            {
                origin = await _context.Origins
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.Id == request.OriginId);
                if (origin == null)
                    throw new CustomException($"Origin ({request.OriginId}) not found");
            }

            Ambit ambit = null;
            if(request.AmbitId != null)
            {
                ambit = await _context.Ambits
                    .AsNoTracking()
                    .Include(x => x.AmbitToOrigins)
                    .Include(x => x.AmbitToTypes.Select(a => a.Type))
                    .FirstOrDefaultAsync(x => x.AmbitToOrigins
                    .Any(o => o.OriginId == request.OriginId) && x.Id == request.AmbitId);

                if (ambit == null)
                    throw new CustomException($"Ambit ({request.AmbitId}) not found");
            }

            IncidentType incidentType = null;
            if (request.IncidentTypeId != null)
            {
                incidentType = ambit?.AmbitToTypes
                    .FirstOrDefault(x => x.TypeId == request.IncidentTypeId)?.Type;
                if (incidentType == null)
                    throw new CustomException($"Incident Type ({request.IncidentTypeId}) not found");
            }

            var incident = await _context.Incidents
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (incident == null)
                throw new CustomException($"Incident ({request.Id}) not found");

            incident.LastModified = DateTime.Now;
            incident.LastModifiedBy = _currentUserService.UserId;
            incident.Subsystem = request.Subsystem;
            incident.Type = Enum.GetName(typeof(RequestType), request.Type);
            incident.ApplicationType = request.ApplicationType;
            incident.Urgency = Enum.GetName(typeof(Urgency), request.Type);
            incident.SubCause = request.SubCause;
            incident.ProblemSumary = request.ProblemSumary;
            incident.ProblemDescription = request.ProblemDescription;
            incident.Solution = request.Solution;
            incident.IncidentTypeId = request.IncidentTypeId;
            incident.AmbitId = request.AmbitId;
            incident.OriginId = request.OriginId;
            incident.ThreatId = request.ThreatId;
            incident.ScenarioId = request.ScenarioId;
            incident.ThirdParty = request.ThirdParty;

            if (DateTime.TryParse(request.OpenDate, out DateTime openDate))
                incident.OpenDate = openDate;

            if (DateTime.TryParse(request.CloseDate, out DateTime closeDate))
                incident.CloseDate = closeDate;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}