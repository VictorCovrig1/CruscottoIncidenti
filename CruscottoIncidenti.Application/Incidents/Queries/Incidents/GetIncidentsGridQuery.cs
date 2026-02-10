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

namespace CruscottoIncidenti.Application.Incidents.Queries.Incidents
{
    public class GetIncidentsGridQuery : IRequest<List<IncidentRowViewModel>>
    {
        public DataTablesParameters Parameters { get; set; }
    }

    public class GetIncidentsGridHandler : IRequestHandler<GetIncidentsGridQuery, List<IncidentRowViewModel>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetIncidentsGridHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<List<IncidentRowViewModel>> Handle(GetIncidentsGridQuery request, CancellationToken cancellationToken)
        {
            return await _context.Incidents
                .AsNoTracking()
                .Where(x => !x.IsDeleted)
                .OrderBy(request.Parameters)
                .Search(request.Parameters)
                .Page(request.Parameters)
                .Select(x => new IncidentRowViewModel
                {
                    Id = x.Id,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDateDT = x.OpenDate,
                    CloseDateDT = x.CloseDate,
                    Type = x.Type,
                    Urgency = x.Urgency
                }).ToListAsync(cancellationToken);
        }
    }
}
