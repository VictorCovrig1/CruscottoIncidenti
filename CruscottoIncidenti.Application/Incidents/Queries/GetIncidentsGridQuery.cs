using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.TableParameters;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries
{
    public class GetIncidentsGridQuery : IRequest<Tuple<int, List<IncidentRowViewModel>>>
    {
        public DataTablesParameters Parameters { get; set; }
    }

    public class GetIncidentsGridHandler : IRequestHandler<GetIncidentsGridQuery, Tuple<int, List<IncidentRowViewModel>>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetIncidentsGridHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Tuple<int, List<IncidentRowViewModel>>> Handle(GetIncidentsGridQuery request, CancellationToken cancellationToken)
        {
            string orderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
            string searchKey = request.Parameters.Search.Value ?? string.Empty;

            var result = await _context.Incidents
                .Where(x => x.RequestNr.Contains(searchKey) ||
                    x.Subsystem.Contains(searchKey) ||
                    x.Type.Contains(searchKey))
                .OrderBy(orderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new IncidentRowViewModel
                {
                    Id = x.Id,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate,
                    Type = x.Type,
                    Urgency = x.Urgency
                }).ToListAsync(cancellationToken);

            int total = await _context.Incidents
                .Where(x => x.RequestNr.Contains(searchKey) ||
                    x.Subsystem.Contains(searchKey) ||
                    x.Type.Contains(searchKey))
                .CountAsync(cancellationToken);

            return new Tuple<int, List<IncidentRowViewModel>>(item1: total, item2: result);
        }
    }
}
