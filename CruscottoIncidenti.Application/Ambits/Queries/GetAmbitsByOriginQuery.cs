using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Ambits.Queries
{
    public class GetAmbitsByOriginQuery : IRequest<Dictionary<string, string>>
    {
        public int OriginId { get; set; }
    }

    public class GetAmbitsPerOriginHandler : IRequestHandler<GetAmbitsByOriginQuery, Dictionary<string, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetAmbitsPerOriginHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<string, string>> Handle(GetAmbitsByOriginQuery request, CancellationToken cancellationToken)
        {
            if (request.OriginId == 0)
                return new Dictionary<string, string>();

            var ambits = await _context.Ambits.Include("Origins").AsNoTracking().
                Where(a => a.Origins.Select(o => o.Id).Contains(request.OriginId)).ToListAsync();

            return ambits.ToDictionary(k => k.Id.ToString(), v => v.Name);
        }
    }
}
