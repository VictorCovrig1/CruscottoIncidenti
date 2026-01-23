using System.Web.Mvc;
using CruscottoIncidenti.Filters;

namespace CruscottoIncidenti
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new CustomErrorHandlerAttribute());
        }
    }
}
