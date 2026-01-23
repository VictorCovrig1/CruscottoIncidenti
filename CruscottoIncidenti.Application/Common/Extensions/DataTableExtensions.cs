using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CruscottoIncidenti.Application.Common
{
    public static class DataTableExtensions
    {
        public static IQueryable<T> OrderBy<T>(this IQueryable<T> source, string ordering, string dir, params object[] values)
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
