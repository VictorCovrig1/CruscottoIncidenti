using System;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class UpdateIncidentCommand : IRequest<bool>
    {
        public int IncidentId { get; set; }

        public int EditorId { get; set; }

        public string Subsystem { get; set; }

        public int Type { get; set; }

        public string ApplicationType { get; set; }

        public int? Urgency { get; set; }

        public string SubCause { get; set; }

        public string ProblemSumary { get; set; }

        public string ProblemDescription { get; set; }

        public string Solution { get; set; }

        public int? IncidentTypeId { get; set; }

        public int? AmbitId { get; set; }

        public int? OriginId { get; set; }

        public int? ThreatId { get; set; }

        public int? ScenarioId { get; set; }

        public string ThirdParty { get; set; }
    }

    public class UpdateIncidentHandler : IRequestHandler<UpdateIncidentCommand, bool>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public UpdateIncidentHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<bool> Handle(UpdateIncidentCommand request, CancellationToken cancellationToken)
        {
            var incident = await _context.Incidents.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.IncidentId);

            if (incident == null)
                throw new CustomException($"Incident ({request.IncidentId}) not found");

            var scenario = await _context.Scenarios.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ScenarioId);
            if(scenario == null)
                throw new CustomException($"Scenario ({request.ScenarioId}) not found");

            var threat = await _context.Threats.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.ScenarioId);
            if (threat == null)
                throw new CustomException($"Threat ({request.ThreatId}) not found");

            var origin = await _context.Origins.AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == request.OriginId);
            if (origin == null)
                throw new CustomException($"Origin ({request.OriginId}) not found");

            var ambit = await _context.Ambits.AsNoTracking().Include("OriginsToAmbit").FirstOrDefaultAsync
                (x => x.AmbitToOrigins.Any(o => o.OriginId == request.OriginId) && x.Id == request.AmbitId);
            if (origin == null)
                throw new CustomException($"Ambit ({request.AmbitId}) not found");

            var incidentType = ambit.AmbitToTypes.FirstOrDefault(x => x.TypeId == request.IncidentId).Type;
            if (incidentType == null)
                throw new CustomException($"Incident Type ({request.IncidentTypeId}) not found");

            incident.LastModified = DateTime.Now;
            incident.LastModifiedBy = request.EditorId;
            incident.Subsystem = request.Subsystem;
            incident.Type = Enum.GetName(typeof(RequestType), request.Type);
            incident.ApplicationType = request.ApplicationType;
            incident.Urgency = request.Urgency;
            incident.SubCause = request.SubCause;
            incident.ProblemSumary = request.ProblemSumary;
            incident.ProblemDescription = request.ProblemDescription;
            incident.Solution = request.Solution;
            incident.IncidentTypeId = request.IncidentTypeId;
            incident.IncidentType = incidentType;
            incident.AmbitId = request.AmbitId;
            incident.Ambit = ambit;
            incident.OriginId = request.OriginId;
            incident.Origin = origin;
            incident.ThreatId = request.ThreatId;
            incident.Threat = threat;
            incident.ScenarioId = request.ScenarioId;
            incident.Scenario = scenario;
            incident.ThirdParty = request.ThirdParty;

            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }
    }
}