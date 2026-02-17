using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.Commands.Common;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;
using Serilog;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class ImportIncidentHandler : IRequestHandler<ImportIncidentsViewModel, (string, List<string>)>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;

        public ImportIncidentHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService, ILogger logger)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
        }

        public async Task<(string, List<string>)> Handle(ImportIncidentsViewModel request, CancellationToken cancellationToken)
        {
            string validationMessage = string.Empty;
            var invalidEntityNames = new List<string>();

            // Select unique elements
            var existentRequestNumbers = _context.Incidents.Select(x => x.RequestNr);
            var newIncidents = request.Incidents
                .Where(x => !existentRequestNumbers.Contains(x.RequestNr));

            // Log dublicates
            var dublicatedIncidents = request.Incidents.Except(newIncidents).Select(x => x.RequestNr);
            if (dublicatedIncidents.Any())
            {
                _logger.Error($"UserId {_currentUserService.UserId} -> Incidents ({string.Join(",", dublicatedIncidents)}) are dublicated");
                validationMessage = "Dublicated incidents found\n";
            }

            var validIncidents = new List<CreateIncidentViewModel>();

            if (newIncidents.Any())
            {
                // Find invalid scenarios and log
                var scenariosIds = await _context.Scenarios
                    .AsNoTracking()
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);
                var invalidScenarios = newIncidents.Where(x => !scenariosIds.Contains(x.ScenarioId))
                    .ToDictionary(k => k.RequestNr, v => v.ScenarioId);

                LogInvalidEntities(invalidScenarios, invalidEntityNames, "Scenarios");

                // Find invalid threats and log
                var threatsIds = await _context.Threats
                    .AsNoTracking()
                    .Select(x => x.Id)
                    .ToListAsync(cancellationToken);
                var invalidThreats = newIncidents.Where(x => !threatsIds.Contains(x.ThreatId))
                    .ToDictionary(k => k.RequestNr, v => v.ThreatId);

                LogInvalidEntities(invalidThreats, invalidEntityNames, "Threats");

                // Find invalid origins and log
                var origins = await _context.Origins.Select(x => x.Id).ToListAsync(cancellationToken);
                var invalidOrigins = newIncidents
                    .Where(x => !origins.Contains(x.OriginId))
                    .ToDictionary(k => k.RequestNr, v => v.OriginId);

                LogInvalidEntities(invalidOrigins, invalidEntityNames, "Origins");

                var invalidAmbits = new Dictionary<string, int>();
                var invalidTypes = new Dictionary<string, int>();

                if (!invalidOrigins.Any())
                {
                    // Find invalid ambits and log
                    var ambitsEntities = await _context.Ambits
                        .Include(x => x.AmbitToOrigins)
                        .Include(x => x.AmbitToTypes)
                        .AsNoTracking()
                        .ToListAsync(cancellationToken);

                    var ambitsOrigins = ambitsEntities.SelectMany(x => x.AmbitToOrigins).GroupBy(x => x.OriginId);
                    invalidAmbits = newIncidents
                        .Where(x => !ambitsOrigins
                        .Any(y => y.Key == x.OriginId && y.Select(a => a.AmbitId)
                        .Contains(x.AmbitId)))
                        .ToDictionary(k => k.RequestNr, v => v.AmbitId);

                    LogInvalidEntities(invalidAmbits, invalidEntityNames, "Ambits");

                    if (!invalidAmbits.Any())
                    {
                        // Find invalid types and log
                        var ambitsTypes = ambitsEntities.SelectMany(x => x.AmbitToTypes).GroupBy(x => x.AmbitId);
                        invalidTypes = newIncidents
                            .Where(x => !ambitsTypes
                            .Any(y => y.Key == x.AmbitId && y.Select(a => a.TypeId)
                            .Contains(x.IncidentTypeId)))
                            .ToDictionary(k => k.RequestNr, v => v.IncidentTypeId);

                        LogInvalidEntities(invalidTypes, invalidEntityNames, "Incident Types");
                    }
                }

                // Generate validation message for invalid entities
                if (invalidEntityNames.Any())
                    validationMessage = $"{validationMessage}{string.Join(", ", invalidEntityNames)} are not valid for one or more incidents";

                // Insert in db only valid incidents
                validIncidents = newIncidents.Where(x =>
                    !invalidScenarios.Select(y => y.Key).Contains(x.RequestNr) &&
                    !invalidThreats.Select(y => y.Key).Contains(x.RequestNr) &&
                    !invalidAmbits.Select(y => y.Key).Contains(x.RequestNr) &&
                    !invalidOrigins.Select(y => y.Key).Contains(x.RequestNr) &&
                    !invalidTypes.Select(y => y.Key).Contains(x.RequestNr))
                    .ToList();

                foreach (var incident in validIncidents)
                {
                    IncidentHelper.InsertIncidentInContext(_currentUserService.UserId, incident, _context.Incidents);
                }

                await _context.SaveChangesAsync(cancellationToken);
            }

            // Return inserted incidents
            return (validationMessage.ToString(), validIncidents.Select(x => x.RequestNr).ToList());
        }

        private void LogInvalidEntities(Dictionary<string, int> invalidEntities, 
            List<string> invalidEntityNames, string entityName)
        {
            if (invalidEntities.Any())
            {
                invalidEntityNames.Add(entityName);
                foreach (var entity in invalidEntities)
                {
                    _logger.Error($"UserId {_currentUserService.UserId} -> RequestNr ({entity.Key}) -> Not found {entityName} ({entity.Value})");
                }
            }
        }
    }
}
