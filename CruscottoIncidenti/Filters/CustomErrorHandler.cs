using System.Net;
using System.Web.Mvc;
using System.Web.Routing;
using Serilog;

namespace CruscottoIncidenti.Filters
{
    public class CustomErrorHandlerAttribute : HandleErrorAttribute
    {
        protected ILogger Logger => DependencyResolver.Current.GetService<ILogger>();

        public override void OnException(ExceptionContext filterContext)
        {
            if (filterContext.ExceptionHandled)
                return;

            Logger.Error(filterContext.Exception, string.Empty);

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = string.Empty,
                    controller = "Error",
                    action = "InternalError"
                }));

                UpdateFilterContext(filterContext);
                return;
            }

            var code = HttpStatusCode.InternalServerError;
            UpdateFilterContext(filterContext, (int)code);
        }

        private static void UpdateFilterContext(ExceptionContext filterContext, int statusCode = (int)HttpStatusCode.InternalServerError)
        {
            filterContext.ExceptionHandled = true;
            filterContext.HttpContext.Response.Clear();
            filterContext.HttpContext.Response.StatusCode = statusCode;
            filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
    }
}