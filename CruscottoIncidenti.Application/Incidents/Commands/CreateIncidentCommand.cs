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
            await IncidentHelper.CheckEntitiesIfExistAsync(_context, request.ScenarioId,
                request.ThreatId, request.OriginId, request.AmbitId, request.IncidentTypeId);

            var dublicatedIncident = await _context.Incidents
                .FirstOrDefaultAsync(x => x.RequestNr == request.RequestNr, cancellationToken);
            if (dublicatedIncident != null)
                throw new CustomException($"Incident with the same request number ({dublicatedIncident.RequestNr}) already exists");

            IncidentHelper.InsertIncidentInContext(_currentUserService.UserId, request, _context.Incidents);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
