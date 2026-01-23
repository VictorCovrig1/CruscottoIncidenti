using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Incidents.Commands.CreateIncident;
using CruscottoIncidenti.Application.Incidents.Queries;
using CruscottoIncidenti.Application.Incidents.Validators;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Utils;
using Microsoft.AspNet.Identity;

namespace CruscottoIncidenti.Controllers
{
    public class IncidentController : BaseController
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetIncidentsGrid(DataTablesParameters parameters)
        {
            var result = await Mediator.Send(new GetIncidentsGridQuery { Parameters = parameters });

            return new JsonCamelCaseResult(new
            {
                Draw = parameters.Draw,
                RecordsFiltered = result.Item1,
                RecordsTotal = result.Item1,
                Data = result.Item2
            });
        }

        [HttpPost]
        public async Task<ActionResult> CreateIncident(CreateIncidentCommand incident)
        {
            incident.CreatorId = int.Parse(HttpContext.User.Identity.GetUserId());

            var validator = new CreateIncidentValidator();
            var validateResult = validator.Validate(incident);

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(incident);
            }

            var isSuccessful = await Mediator.Send(incident);

            if (isSuccessful)
                return RedirectToAction("Index");
            else
                return View(incident);
        }
    }
}