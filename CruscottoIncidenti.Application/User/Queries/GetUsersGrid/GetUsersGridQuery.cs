using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
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

    public class GetUsersHandler : IRequestHandler<GetUsersGridQuery, Tuple<int, List<UserRowViewModel>>>
    {
        private readonly ICruscottoIncidentiDbContext _context;

        public GetUsersHandler(ICruscottoIncidentiDbContext context)
            => _context = context;

        public async Task<Tuple<int, List<UserRowViewModel>>> Handle(GetUsersGridQuery request, CancellationToken cancellationToken)
        {
            string orderColumn = request.Parameters.Columns[request.Parameters.Order[0].Column].Name;
            string searchKey = request.Parameters.Search.Value ?? string.Empty;

            var result = await _context.Users
                .Include("Roles")
                .Where(x => x.UserName.Contains(searchKey) || x.Email.Contains(searchKey))
                .OrderByExtension(orderColumn, request.Parameters.Order[0].Dir)
                .Skip(request.Parameters.Start)
                .Take(request.Parameters.Length)
                .Select(x => new UserRowViewModel
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    IsEnabled = x.IsEnabled,
                }).ToListAsync(cancellationToken);

            int total = await _context.Users
                .Where(x => x.UserName.Contains(searchKey) || x.Email.Contains(searchKey))
                .CountAsync(cancellationToken);

            return new Tuple<int, List<UserRowViewModel>>(item1: total, item2: result);
        }
    }

    public static class LinqHelper
    {
        public static IQueryable<T> OrderByExtension<T>(this IQueryable<T> source, string ordering, string dir, params object[] values)
        {
            var type = typeof(T);
            var property = type.GetProperty(ordering, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            var parameter = Expression.Parameter(type, "p");
            var propertyAccess = Expression.MakeMemberAccess(parameter, property);
            var orderByExp = Expression.Lambda(propertyAccess, parameter);
            MethodCallExpression resultExp = Expression.Call(typeof(Queryable), dir == "asc" ? "OrderBy" : "OrderByDescending", 
                new Type[] { type, property.PropertyType }, source.Expression, Expression.Quote(orderByExp));

            return source.Provider.CreateQuery<T>(resultExp);
        }
    }
}
