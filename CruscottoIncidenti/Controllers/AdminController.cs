using System.Web.Mvc;

namespace CruscottoIncidenti.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : BaseController
    {
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}