using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper _mapper;


        public GetIncidentsGridHandler(ICruscottoIncidentiDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Tuple<int, List<IncidentRowViewModel>>> Handle(GetIncidentsGridQuery request, CancellationToken cancellationToken)
        {
            string orderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
            string searchKey = request.Parameters.Search.Value ?? string.Empty;

            var incidents = await _context.Incidents
                .AsNoTracking()
                .Where(x => (x.RequestNr.Contains(searchKey) || x.Subsystem.Contains(searchKey) 
                    || x.Type.Contains(searchKey)) && !x.IsDeleted)
                .OrderBy(orderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new
                {
                    Id = x.Id,
                    RequestNr = x.RequestNr,
                    Subsystem = x.Subsystem,
                    OpenDate = x.OpenDate,
                    CloseDate = x.CloseDate,
                    Type = x.Type,
                    Urgency = x.Urgency
                }).ToListAsync(cancellationToken);

            var result = new List<IncidentRowViewModel>();
            foreach(var incident in incidents)
            {
                result.Add(new IncidentRowViewModel()
                {
                    Id = incident.Id,
                    RequestNr = incident.RequestNr,
                    Subsystem = incident.Subsystem,
                    OpenDate = incident.OpenDate.ToString("dd/MM/yyyy"),
                    CloseDate = incident.CloseDate != null ? 
                        incident.CloseDate.Value.ToString("dd/MM/yyyy") : 
                        null,
                    Type = incident.Type,
                    Urgency = incident.Urgency
                });
            }

            int total = await _context.Incidents.AsNoTracking()
                    .Where(x => (x.RequestNr.Contains(searchKey) || x.Subsystem.Contains(searchKey) 
                        || x.Type.Contains(searchKey)) && !x.IsDeleted)
                    .CountAsync(cancellationToken);

            return new Tuple<int, List<IncidentRowViewModel>>(item1: total, item2: result);
        }
    }
}
