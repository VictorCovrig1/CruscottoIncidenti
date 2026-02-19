using System;
using System.Data.Entity;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Domain.Entities;

namespace CruscottoIncidenti.Application.Incidents.Commands.Common
{
    public static class IncidentHelper
    {
        public static void InsertDateToIncident(string date, Incident incident)
        {
            var acceptableFormats = new string[] { "dd/MM/yyyy", "d/MM/yyyy", "dd/M/yyyy" };

            if (DateTime.TryParseExact(date, acceptableFormats,
                CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime openDate))
            {
                incident.OpenDate = openDate;
            }
        }
        
        public static async Task CheckEntitiesIfExistAsync(ICruscottoIncidentiDbContext context, 
            int? scenarioId, int? threatId, int? originId, int? ambitId, int? incidentTypeId)
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
