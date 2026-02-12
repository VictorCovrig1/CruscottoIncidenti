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

            if (!filterContext.HttpContext.Request.IsAjaxRequest())
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = string.Empty,
                    controller = "Error",
                    action = "InternalError"
                }));

                UpdateFilterContext(filterContext);
                Logger.Error(filterContext.Exception, string.Empty);
                return;
            }

            var code = HttpStatusCode.InternalServerError;
            //ExceptionJsonResponse result = new ExceptionJsonResponse();

            //if (string.IsNullOrEmpty(result.Message))
            //{
            //    result = new ExceptionJsonResponse { Message = "Internal Server Error, Contattare l'amministratore.", StackTrace = filterContext.Exception.StackTrace };
            //    Logger.Error(filterContext.Exception, "");
            //}

            //filterContext.Result = new JsonResult
            //{
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
            //    Data = result
            //};

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