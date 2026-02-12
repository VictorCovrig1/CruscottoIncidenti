using System.Web.Mvc;

namespace CruscottoIncidenti.Controllers
{
    public class ErrorController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public ActionResult InternalError()
        {
            return View("~/Views/Shared/500.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Unauthorized()
        {
            return View("~/Views/Shared/401.cshtml");
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult NotFound()
        {
            return View("~/Views/Shared/404.cshtml");
        }
    }
}