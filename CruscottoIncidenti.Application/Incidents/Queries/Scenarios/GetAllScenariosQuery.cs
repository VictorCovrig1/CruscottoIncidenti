using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Queries.Scenarios
{
    public class GetAllScenariosQuery : IRequest<Dictionary<string, string>>
    {
    }

    public class GetAllScenariosHandler : IRequestHandler<GetAllScenariosQuery, Dictionary<string, string>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetAllScenariosHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Dictionary<string, string>> Handle(GetAllScenariosQuery request, CancellationToken cancellationToken)
        {
            var scenarios = await _context.Scenarios.AsNoTracking().ToListAsync();

            return scenarios.ToDictionary(k => k.Id.ToString(), v => v.Name);
        }
    }
}
