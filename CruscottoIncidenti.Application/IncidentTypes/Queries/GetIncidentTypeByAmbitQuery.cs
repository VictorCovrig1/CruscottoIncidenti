using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.IncidentTypes.Queries
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

            var ambit = await _context.Ambits.Include("IncidentTypes").AsNoTracking().
                FirstOrDefaultAsync(a => a.Id == request.AmbitId);

            return ambit.IncidentTypes.ToList().ToDictionary(k => k.Id.ToString(), v => v.Name);
        }
    }
}
