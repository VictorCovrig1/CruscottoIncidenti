using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Threats.Queries
{
    public class GetAllThreatsQuery : IRequest<Dictionary<int, string>>
    {
    }

    public class GetAllThreatsHandler : IRequestHandler<GetAllThreatsQuery, Dictionary<int, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetAllThreatsHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<int, string>> Handle(GetAllThreatsQuery request, CancellationToken cancellationToken)
        {
            var threats = await _context.Threats.AsNoTracking().ToListAsync();

            return threats.ToDictionary(k => k.Id, v => v.Name);
        }
    }
}
