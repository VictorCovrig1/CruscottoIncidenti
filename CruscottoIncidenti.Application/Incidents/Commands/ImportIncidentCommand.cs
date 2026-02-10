using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Incidents.Commands.Common;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class ImportIncidentHandler : IRequestHandler<ImportIncidentsViewModel, Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private ICurrentUserService _currentUserService;
        private IEqualityComparer<CreateIncidentViewModel> _comparer;

        public ImportIncidentHandler(ICruscottoIncidentiDbContext context, 
            ICurrentUserService currentUserService, IEqualityComparer<CreateIncidentViewModel> comparer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _comparer = comparer;
        }

        public async Task<Unit> Handle(ImportIncidentsViewModel request, CancellationToken cancellationToken)
        {
            var existentRequestNumbers = _context.Incidents.Select(x => x.RequestNr);
            var newIncidents = request.Incidents.Where(x => !existentRequestNumbers.Contains(x.RequestNr)).Distinct(_comparer);

            // Scenarios
            var scenariosIds = await _context.Scenarios
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
            var invalidScenarios = request.Incidents.Select(x => x.ScenarioId).Where(x => !scenariosIds.Contains(x));

            if (invalidScenarios.Count() > 0)
                throw new CustomException($"Scenarios ({string.Join(",", invalidScenarios)}) not found");

            // Threats
            var threatsIds = await _context.Threats
                .AsNoTracking()
                .Select(x => x.Id)
                .ToListAsync(cancellationToken);
            var invalidThreats = request.Incidents.Select(x => x.ThreatId).Where(x => !threatsIds.Contains(x));

            if (invalidThreats.Count() > 0)
                throw new CustomException($"Threats ({string.Join(",", invalidThreats)}) not found");

            // Ambits
            var ambitsEntities = await _context.Ambits
                .Include(x => x.AmbitToOrigins)
                .Include(x => x.AmbitToTypes)
                .AsNoTracking()
                .ToListAsync(cancellationToken);

            var ambitsIds = ambitsEntities.Select(x => x.Id);
            var invalidAmbits = request.Incidents.Where(x => !ambitsIds.Contains(x.AmbitId));

            if (invalidAmbits.Count() > 0)
                throw new CustomException($"Ambits ({string.Join(",", invalidAmbits.Select(x => x.AmbitId))}) not found");

            // Origins
            var ambitsOrigins = ambitsEntities.SelectMany(x => x.AmbitToOrigins).GroupBy(x => x.AmbitId);
            var origins = await _context.Origins.Select(x => x.Id).ToListAsync(cancellationToken);
            var invalidOrigins = request.Incidents
                .Where(x => !ambitsOrigins
                .Any(y => y.Key == x.AmbitId && y.Select(a => a.OriginId)
                .Contains(x.OriginId)) || !origins.Contains(x.OriginId))
                .Select(x => x.OriginId);

            if (invalidOrigins.Count() > 0)
                throw new CustomException($"Origins ({string.Join(",", invalidOrigins)}) not found");

            // Incidents
            var ambitsTypes = ambitsEntities.SelectMany(x => x.AmbitToTypes).GroupBy(x => x.AmbitId);
            var types = await _context.IncidentTypes.Select(x => x.Id).ToListAsync(cancellationToken);
            var invalidTypes = request.Incidents
                .Where(x => !ambitsTypes
                .Any(y => y.Key == x.AmbitId && y.Select(a => a.TypeId)
                .Contains(x.IncidentTypeId)) || !types.Contains(x.IncidentTypeId))
                .Select(x => x.IncidentTypeId);

            if (invalidTypes.Count() > 0)
                throw new CustomException($"Incident Types ({string.Join(",", invalidTypes)}) not found");

            foreach (var newIncident in newIncidents)
            {
                IncidentHelper.InsertIncidentInContext(_currentUserService.UserId, newIncident, _context.Incidents);
            }

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
