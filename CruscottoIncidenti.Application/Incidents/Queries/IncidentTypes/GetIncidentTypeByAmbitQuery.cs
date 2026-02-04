using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries.IncidentTypes
{
    public class GetIncidentTypeByAmbitQuery : IRequest<Dictionary<string, string>>
    {
        public int AmbitId { get; set; }
    }

    public class GetIncidentTypeByAmbitHandler : IRequestHandler<GetIncidentTypeByAmbitQuery, Dictionary<string, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetIncidentTypeByAmbitHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<string, string>> Handle(GetIncidentTypeByAmbitQuery request, CancellationToken cancellationToken)
        {
            if (request.AmbitId == 0)
                return new Dictionary<string, string>();

            var ambit = await _context.Ambits
                .Include(x => x.AmbitToTypes
                .Select(a => a.Type))
                .AsNoTracking()
                .FirstOrDefaultAsync(a => a.Id == request.AmbitId);

            return ambit.AmbitToTypes.ToList()
                .ToDictionary(k => k.TypeId
                .ToString(), v => v.Type.Name);
        }
    }
}
