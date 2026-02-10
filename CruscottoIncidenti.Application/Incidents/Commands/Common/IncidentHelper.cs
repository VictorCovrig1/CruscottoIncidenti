using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Application.Incidents.Commands.Common
{
    public static class IncidentHelper
    {
        public static void InsertIncidentInContext(int currentUserId, CreateIncidentViewModel newIncident, DbSet<Incident> contextIncidents)
        {
            var incident = new Incident
            {
                CreatedBy = currentUserId,
                Created = DateTime.UtcNow,
                RequestNr = newIncident.RequestNr,
                Subsystem = newIncident.Subsystem,
                Type = Enum.GetName(typeof(RequestType), newIncident.Type),
                Urgency = Enum.GetName(typeof(Urgency), newIncident.Type),
                SubCause = newIncident.SubCause,
                ProblemSumary = newIncident.ProblemSummary,
                ProblemDescription = newIncident.ProblemDescription,
                Solution = newIncident.Solution,
                IncidentTypeId = newIncident.IncidentTypeId,
                AmbitId = newIncident.AmbitId,
                OriginId = newIncident.OriginId,
                ThreatId = newIncident.ThreatId,
                ScenarioId = newIncident.ScenarioId,
                ThirdParty = newIncident.ThirdParty,
                ApplicationType = newIncident.ApplicationType
            };

            if (DateTime.TryParse(newIncident.OpenDate, out DateTime openDate))
                incident.OpenDate = openDate;

            if (DateTime.TryParse(newIncident.CloseDate, out DateTime closeDate))
                incident.CloseDate = closeDate;

            contextIncidents.Add(incident);
        }
        
        public static async Task CheckEntitiesIfExistAsync(ICruscottoIncidentiDbContext context, 
            int scenarioId, int threatId, int originId, int ambitId, int incidentTypeId)
        {
            Scenario scenario = await context.Scenarios
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == scenarioId);
            if (scenario == null)
                throw new CustomException($"Scenario ({scenarioId}) not found");

            Threat threat = await context.Threats
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == threatId);
            if (threat == null)
                throw new CustomException($"Threat ({threatId}) not found");

            Origin origin = await context.Origins
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == originId);
            if (origin == null)
                throw new CustomException($"Origin ({originId}) not found");

            Ambit ambit = await context.Ambits
                .AsNoTracking()
                .Include(x => x.AmbitToOrigins)
                .Include(x => x.AmbitToTypes.Select(a => a.Type))
                .FirstOrDefaultAsync(x => x.AmbitToOrigins
                .Any(o => o.OriginId == originId) && x.Id == ambitId);
            if (ambit == null)
                throw new CustomException($"Ambit ({ambitId}) not found");

            IncidentType incidentType = ambit.AmbitToTypes
                .FirstOrDefault(x => x.TypeId == incidentTypeId)?.Type;
            if (incidentType == null)
                throw new CustomException($"Incident Type ({incidentTypeId}) not found");
        }
    }
}
