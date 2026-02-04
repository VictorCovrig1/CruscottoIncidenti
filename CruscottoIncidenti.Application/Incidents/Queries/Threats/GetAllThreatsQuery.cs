using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries.Threats
{
    public class GetAllThreatsQuery : IRequest<Dictionary<string, string>>
    {
    }

    public class GetAllThreatsHandler : IRequestHandler<GetAllThreatsQuery, Dictionary<string, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetAllThreatsHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<string, string>> Handle(GetAllThreatsQuery request, CancellationToken cancellationToken)
        {
            var threats = await _context.Threats.AsNoTracking().ToListAsync();

            return threats.ToDictionary(k => k.Id.ToString(), v => v.Name);
        }
    }
}
