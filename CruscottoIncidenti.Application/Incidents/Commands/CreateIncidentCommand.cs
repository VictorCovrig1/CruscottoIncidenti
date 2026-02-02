using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
            string lastRequestNumber = _context.Incidents.AsNoTracking()
                .OrderByDescending(i => i.Id).FirstOrDefault()?.RequestNr;

            string requestNumber = Constants.DefaultRequestNr;
            if (lastRequestNumber != null && int.TryParse(lastRequestNumber.Substring(4), out int uniqueNumber))
            {
                string incrementedRequestNumber = (uniqueNumber + 1).ToString();
                requestNumber = $"{Constants.RequestNrPRefix}{incrementedRequestNumber.PadLeft(13, '0')}";
            }
            
            var incident = new Incident
            {
                CreatedBy = _currentUserService.UserId,
                Created = DateTime.UtcNow,
                RequestNr = requestNumber,
                Subsystem = request.Subsystem,
                OpenDate = DateTime.UtcNow,
                Type = Enum.GetName(typeof(RequestType), request.Type),
                Urgency = Enum.GetName(typeof(Urgency), request.Type),
                SubCause = request.SubCause,
                ProblemSumary = request.ProblemSumary,
                ProblemDescription = request.ProblemDescription,
                IncidentTypeId = request.IncidentTypeId,
                AmbitId = request.AmbitId,
                OriginId = request.OriginId,
                ThreatId = request.ThreatId,
                ScenarioId = request.ScenarioId,
                ThirdParty = request.ThirdParty
            };

            _context.Incidents.Add(incident);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
