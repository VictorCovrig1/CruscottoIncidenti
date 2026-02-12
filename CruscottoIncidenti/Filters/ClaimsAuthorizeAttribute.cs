using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web.Mvc;
using System.Web.Routing;
using CruscottoIncidenti.Common;

namespace CruscottoIncidenti.Filters
{
    public class ClaimsAuthorizeAttribute : AuthorizeAttribute
    {
        private readonly IEnumerable<Role> claimValues;

        public ClaimsAuthorizeAttribute(params Role[] values)
        {
            claimValues = values;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            ClaimsPrincipal user = filterContext.HttpContext.User as ClaimsPrincipal;
            bool isAuthorized = claimValues.Any(item => user.HasClaim(ClaimTypes.Role, item.ToString()));

            if (user != null && isAuthorized)
            {
                base.OnAuthorization(filterContext);
            }
            else
            {
                this.HandleUnauthorizedRequest(filterContext);
            }
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new
                {
                    area = string.Empty,
                    controller = "Error",
                    action = "Unauthorized"
                }));

                return;
            }

            base.HandleUnauthorizedRequest(filterContext);
        }
    }
}