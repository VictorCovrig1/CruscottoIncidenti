using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CruscottoIncidenti.Application.Common;
using CruscottoIncidenti.Application.Interfaces;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.User.ViewModels;
using MediatR;

namespace CruscottoIncidenti.Application.User.Queries.GetUsers
{
    public class GetUsersGridQuery : IRequest<Tuple<int, List<UserRowViewModel>>>
    {
        public DataTablesParameters Parameters { get; set; }
    }

    public class GetUsersGridHandler : IRequestHandler<GetUsersGridQuery, Tuple<int, List<UserRowViewModel>>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUsersGridHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Tuple<int, List<UserRowViewModel>>> Handle(GetUsersGridQuery request, CancellationToken cancellationToken)
        {
            string orderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
            string searchKey = request.Parameters.Search.Value ?? string.Empty;

            var result = await _context.Users
                .Include("Roles")
                .Where(x => x.UserName.Contains(searchKey) || x.Email.Contains(searchKey))
                .OrderBy(orderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new UserRowViewModel
                {
                    Id = x.Id,
                    Username = x.UserName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                }).ToListAsync(cancellationToken);

            int total = await _context.Users
                .Where(x => x.UserName.Contains(searchKey) || x.Email.Contains(searchKey))
                .CountAsync(cancellationToken);

            return new Tuple<int, List<UserRowViewModel>>(item1: total, item2: result);
        }
    }
}
