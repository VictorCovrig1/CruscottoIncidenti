using System;
using System.Data.Entity;
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
    public class CreateIncidentHandler : IRequestHandler<CreateIncidentViewModel, Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private ICurrentUserService _currentUserService;

        public CreateIncidentHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(CreateIncidentViewModel request, CancellationToken cancellationToken)
        {
            var dublicatedIncident = await _context.Incidents
                .FirstOrDefaultAsync(x => x.RequestNr == request.RequestNr, cancellationToken);

            if (dublicatedIncident != null)
                throw new CustomException($"Incident with the same request number ({dublicatedIncident.RequestNr}) already exists");

            var incident = new Incident
            {
                CreatedBy = _currentUserService.UserId,
                Created = DateTime.UtcNow,
                RequestNr = request.RequestNr,
                Subsystem = request.Subsystem,
                Type = Enum.GetName(typeof(RequestType), request.Type),
                Urgency = Enum.GetName(typeof(Urgency), request.Type),
                SubCause = request.SubCause,
                ProblemSumary = request.ProblemSummary,
                ProblemDescription = request.ProblemDescription,
                Solution = request.Solution,
                IncidentTypeId = request.IncidentTypeId,
                AmbitId = request.AmbitId,
                OriginId = request.OriginId,
                ThreatId = request.ThreatId,
                ScenarioId = request.ScenarioId,
                ThirdParty = request.ThirdParty,
                ApplicationType = request.ApplicationType
            };

            if (DateTime.TryParse(request.OpenDate, out DateTime openDate))
                incident.OpenDate = openDate;

            if (DateTime.TryParse(request.CloseDate, out DateTime closeDate))
                incident.CloseDate = closeDate;

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
