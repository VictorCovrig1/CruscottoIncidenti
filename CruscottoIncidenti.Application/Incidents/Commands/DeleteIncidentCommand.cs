using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Interfaces;
using MediatR;

namespace CruscottoIncidenti.Application.Incidents.Commands
{
    public class DeleteIncidentCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteIncidentHandler : IRequestHandler<DeleteIncidentCommand, Unit>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public DeleteIncidentHandler(ICruscottoIncidentiDbContext context)
         => _context = context;

        public async Task<Unit> Handle(DeleteIncidentCommand request, CancellationToken cancellationToken)
        {
            var incident = await _context.Incidents.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (incident == null)
                throw new CustomException($"Incident ({request.Id}) not found");

            incident.IsDeleted = true;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
