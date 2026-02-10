using System;
using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Incidents.Commands.Common;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
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
            await IncidentHelper.CheckEntitiesIfExistAsync(_context, request.ScenarioId,
                request.ThreatId, request.OriginId, request.AmbitId, request.IncidentTypeId);

            var incident = await _context.Incidents.FirstOrDefaultAsync(x => x.Id == request.Id);
            if (incident == null)
                throw new CustomException($"Incident ({request.Id}) not found");

            incident.LastModified = DateTime.Now;
            incident.LastModifiedBy = _currentUserService.UserId;
            incident.Subsystem = request.Subsystem;
            incident.Type = request.Type;
            incident.ApplicationType = request.ApplicationType;
            incident.Urgency = request.Urgency;
            incident.SubCause = request.SubCause;
            incident.ProblemSumary = request.ProblemSummary;
            incident.ProblemDescription = request.ProblemDescription;
            incident.Solution = request.Solution;
            incident.IncidentTypeId = request.IncidentTypeId;
            incident.AmbitId = request.AmbitId;
            incident.OriginId = request.OriginId;
            incident.ThreatId = request.ThreatId;
            incident.ScenarioId = request.ScenarioId;
            incident.ThirdParty = request.ThirdParty;
            incident.OpenDate = request.OpenDate;
            incident.CloseDate = request.CloseDate;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}