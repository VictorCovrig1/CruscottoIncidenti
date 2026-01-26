using System;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Incidents.Commands.CreateIncident;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Domain.Entities;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Commands.UpdateIncident
{
    public class UpdateIncidentCommand
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

    //public class CreateIncidentHandler : IRequestHandler<CreateIncidentCommand, bool>
    //{
    //    private readonly ICruscottoIncidentiDbContext _context;

    //    public CreateIncidentHandler(ICruscottoIncidentiDbContext context)
    //        => _context = context;

    //    public async Task<bool> Handle(CreateIncidentCommand request, CancellationToken cancellationToken)
    //    {
    //        string lastRequestNumber = _context.Incidents.OrderByDescending(i => i.Id).FirstOrDefault()?.RequestNr;
    //        string requestNumber = Constants.DefaultRequestNr;

    //        if (lastRequestNumber != null && int.TryParse(lastRequestNumber.Substring(4), out int uniqueNumber))
    //        {
    //            string incrementedRequestNumber = (uniqueNumber + 1).ToString();
    //            requestNumber = $"{Constants.RequestNrPRefix}{incrementedRequestNumber.PadLeft(13, '0')}";
    //        }

    //        var incident = new Incident
    //        {
    //            CreatedBy = request.CreatorId,
    //            Created = DateTime.UtcNow,
    //            RequestNr = requestNumber,
    //            Subsystem = request.Subsystem,
    //            OpenDate = DateTime.UtcNow,
    //            Type = Enum.GetName(typeof(RequestType), request.Type),
    //            Urgency = request.Urgency,
    //            SubCause = request.SubCause,
    //            ProblemSumary = request.ProblemSumary,
    //            ProblemDescription = request.ProblemDescription,
    //            IncidentTypeId = request.IncidentTypeId,
    //            AmbitId = request.AmbitId,
    //            OriginId = request.OriginId,
    //            ThreatId = request.ThreatId,
    //            ScenarioId = request.ScenarioId,
    //            ThirdParty = request.ThirdParty
    //        };

    //        _context.Incidents.Add(incident);

    //        return await _context.SaveChangesAsync(cancellationToken) > 0;
    //    }
    //}

}