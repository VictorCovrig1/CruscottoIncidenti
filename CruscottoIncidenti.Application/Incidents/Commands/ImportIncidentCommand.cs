using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.Commands.Common;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Domain.Entities;
using MediatR;
using Serilog;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class ImportIncidentHandler : IRequestHandler<ImportIncidentsViewModel, (string, List<string>)>
    {
        private readonly ICruscottoIncidentiDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly ILogger _logger;
        private readonly IEqualityComparer<string> _comparer;

        public ImportIncidentHandler(ICruscottoIncidentiDbContext context, ICurrentUserService currentUserService, 
            ILogger logger, IEqualityComparer<string> comparer)
        {
            _context = context;
            _currentUserService = currentUserService;
            _logger = logger;
            _comparer = comparer;
        }

        public async Task<(string, List<string>)> Handle(ImportIncidentsViewModel request, CancellationToken cancellationToken)
        {
            var validationMessages = new List<string>();
            var invalidEntityNames = new List<string>();

            // Validate incidents by field values
            var validIncidents = request.Incidents.Where(x =>
                !string.IsNullOrWhiteSpace(x.RequestNr) && x.RequestNr.Count() == 17 &&
                !string.IsNullOrWhiteSpace(x.Subsystem) && x.Subsystem.Count() == 2 &&
                !string.IsNullOrWhiteSpace(x.Type) && 
                    Enum.GetNames(typeof(RequestType)).Contains(x.Type, _comparer) &&
                !string.IsNullOrWhiteSpace(x.Urgency) && 
                    Enum.GetNames(typeof(Urgency)).Contains(x.Urgency, _comparer) &&
                !string.IsNullOrWhiteSpace(x.ApplicationType) && x.ApplicationType.Count() <= 50 &&
                !string.IsNullOrWhiteSpace(x.SubCause) && x.SubCause.Count() <= 100 &&
                !string.IsNullOrWhiteSpace(x.ProblemSummary) && x.ProblemSummary.Count() <= 500 &&
                !string.IsNullOrWhiteSpace(x.ProblemDescription) &&
                !string.IsNullOrWhiteSpace(x.ThirdParty) && x.ThirdParty.Count() <= 100);

            if (validIncidents.Count() != request.Incidents.Count())
            {
                var invalidIncidents = request.Incidents
                    .Except(validIncidents)
                    .Select(x => x.RequestNr);

                _logger.Error($"Username: {_currentUserService.UserName} -> Incidents " +
                    $"({string.Join(",", invalidIncidents)}) are invalid by one or more fields");

                validationMessages.Add("Invalid incidents found by one or more required columns\n");
            }

            if(!validIncidents.Any())
                return (string.Join(string.Empty, validationMessages), new List<string>());

            // Select unique elements
            var existentRequestNumbers = _context.Incidents.Select(x => x.RequestNr);
            validIncidents = validIncidents
                .Where(x => !existentRequestNumbers.Contains(x.RequestNr));

            // Log dublicates
            var dublicatedIncidents = request.Incidents
                .Except(validIncidents)
                .Select(x => x.RequestNr);

            if (dublicatedIncidents.Any())
            {
                _logger.Error($"Username: {_currentUserService.UserName} -> Incidents " +
                    $"({string.Join(",", dublicatedIncidents)}) are dublicated");

                validationMessages.Add("Dublicated incidents found\n");
            }

            if(!validIncidents.Any())
                return (string.Join(string.Empty, validationMessages), new List<string>());

            // Find invalid scenarios and log
            var scenarios = await _context.Scenarios
                .AsNoTracking()
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(cancellationToken);
            var invalidScenarios = validIncidents
                .Where(x => !scenarios.Select(y => y.Name).Contains(x.Scenario, _comparer))
                .ToDictionary(k => k.RequestNr, v => v.Scenario);

            LogInvalidEntities(invalidScenarios, invalidEntityNames, "Scenarios");

            // Find invalid threats and log
            var threats = await _context.Threats
                .AsNoTracking()
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(cancellationToken);
            var invalidThreats = validIncidents
                .Where(x => !threats.Select(y => y.Name).Contains(x.Threat, _comparer))
                .ToDictionary(k => k.RequestNr, v => v.Threat);

            LogInvalidEntities(invalidThreats, invalidEntityNames, "Threats");

            // Find invalid origins and log
            var origins = await _context.Origins
                .Select(x => new { x.Id, x.Name })
                .ToListAsync(cancellationToken);
            var invalidOrigins = validIncidents
                .Where(x => !origins.Select(y => y.Name).Contains(x.Origin, _comparer))
                .ToDictionary(k => k.RequestNr, v => v.Origin);

            validIncidents = validIncidents.Where(x => !invalidOrigins.Keys.Contains(x.RequestNr));
            LogInvalidEntities(invalidOrigins, invalidEntityNames, "Origins");

            var invalidAmbits = new Dictionary<string, string>();
            var invalidTypes = new Dictionary<string, string>();
            var ambits = new List<Ambit>();

            if (validIncidents.Any())
            {
                // Find invalid ambits and log
                ambits = await _context.Ambits
                    .Include(x => x.AmbitToOrigins.Select(y => y.Origin))
                    .Include(x => x.AmbitToTypes.Select(y => y.Type))
                    .AsNoTracking()
                    .ToListAsync(cancellationToken);

                var ambitsOrigins = ambits
                    .SelectMany(x => x.AmbitToOrigins)
                    .GroupBy(x => x.Origin.Name);
                invalidAmbits = validIncidents
                    .Where(x => !ambitsOrigins
                    .Any(y => y.Key.Contains(x.Origin) && y.Select(a => a.Ambit.Name)
                    .Contains(x.Ambit, _comparer)))
                    .ToDictionary(k => k.RequestNr, v => v.Ambit);

                LogInvalidEntities(invalidAmbits, invalidEntityNames, "Ambits");
                validIncidents = validIncidents.Where(x => !invalidAmbits.Keys.Contains(x.RequestNr));

                if (validIncidents.Any())
                {
                    // Find invalid types and log
                    var ambitsTypes = ambits
                        .SelectMany(x => x.AmbitToTypes)
                        .GroupBy(x => x.Ambit.Name);
                    invalidTypes = validIncidents
                        .Where(x => !ambitsTypes
                        .Any(y => y.Key.Contains(x.Ambit) && y.Select(a => a.Type.Name)
                        .Contains(x.IncidentType, _comparer)))
                        .ToDictionary(k => k.RequestNr, v => v.IncidentType);

                    LogInvalidEntities(invalidTypes, invalidEntityNames, "Incident Types");
                }
            }

            // Generate validation message for invalid entities
            if (invalidEntityNames.Any())
                validationMessages.Add($"{string.Join(", ", invalidEntityNames)} " +
                    "are not valid for one or more incidents");

            // Insert in db only valid incidents
            validIncidents = validIncidents.Where(x =>
                !invalidScenarios.Select(y => y.Key).Contains(x.RequestNr) &&
                !invalidThreats.Select(y => y.Key).Contains(x.RequestNr) &&
                !invalidTypes.Select(y => y.Key).Contains(x.RequestNr))
                .ToList();

            foreach (var validIncident in validIncidents)
            {
                var incident = new Incident
                {
                    CreatedBy = _currentUserService.UserId,
                    Created = DateTime.UtcNow,
                    RequestNr = validIncident.RequestNr,
                    Subsystem = validIncident.Subsystem,
                    Type = Enum.GetNames(typeof(RequestType)).FirstOrDefault(y => y.Contains(validIncident.Type)),
                    Urgency = Enum.GetNames(typeof(Urgency)).FirstOrDefault(y => y.Contains(validIncident.Urgency)),
                    SubCause = validIncident.SubCause,
                    ProblemSumary = validIncident.ProblemSummary,
                    ProblemDescription = validIncident.ProblemDescription,
                    Solution = validIncident.Solution,
                    IncidentTypeId = ambits.SelectMany(x => x.AmbitToTypes)
                        .FirstOrDefault(x => x.Type.Name.Contains(validIncident.IncidentType)).TypeId,
                    AmbitId = ambits.FirstOrDefault(x => x.Name.Contains(validIncident.Ambit)).Id,
                    OriginId = origins.FirstOrDefault(x => x.Name.Contains(validIncident.Origin)).Id,
                    ThreatId = threats.FirstOrDefault(x => x.Name.Contains(validIncident.Threat)).Id,
                    ScenarioId = scenarios.FirstOrDefault(x => x.Name.Contains(validIncident.Scenario)).Id,
                    ThirdParty = validIncident.ThirdParty,
                    ApplicationType = validIncident.ApplicationType
                };

                IncidentHelper.InsertDateToIncident(validIncident.OpenDate, incident);
                IncidentHelper.InsertDateToIncident(validIncident.CloseDate, incident);

                _context.Incidents.Add(incident);
            }

            await _context.SaveChangesAsync(cancellationToken);

            // Return inserted incidents
            return (string.Join(string.Empty, validationMessages), validIncidents.Select(x => x.RequestNr).ToList());
        }

        private void LogInvalidEntities(Dictionary<string, string> invalidEntities, 
            List<string> invalidEntityNames, string entityName)
        {
            if (invalidEntities.Any())
            {
                invalidEntityNames.Add(entityName);
                foreach (var entity in invalidEntities)
                {
                    _logger.Error($"Username: {_currentUserService.UserName} -> RequestNr " +
                        $"({entity.Key}) -> Not found {entityName} ({entity.Value})");
                }
            }
        }
    }
}
