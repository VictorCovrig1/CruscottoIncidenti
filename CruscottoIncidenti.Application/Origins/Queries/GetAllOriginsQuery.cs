using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Origins.Queries
{
    public class GetAllOriginsQuery : IRequest<Dictionary<int, string>>
    {
    }

    public class GetAllOriginsHandler : IRequestHandler<GetAllOriginsQuery, Dictionary<int, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetAllOriginsHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<int, string>> Handle(GetAllOriginsQuery request, CancellationToken cancellationToken)
        {
            var origins = await _context.Origins.AsNoTracking().ToListAsync();

            return origins.ToDictionary(k => k.Id, v => v.Name);
        }
    }
}
