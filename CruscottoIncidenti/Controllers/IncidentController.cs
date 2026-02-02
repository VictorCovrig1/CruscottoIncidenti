using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Ambits.Queries;
using CruscottoIncidenti.Application.Incidents.Queries;
using CruscottoIncidenti.Application.Incidents.Validators;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.IncidentTypes.Queries;
using CruscottoIncidenti.Application.Origins.Queries;
using CruscottoIncidenti.Application.Scenarios.Queries;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.Threats.Queries;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Utils;

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

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = result.Item1,
                recordsTotal = result.Item1,
                data = result.Item2
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetDetailedIncident(int id, bool? shouldBeDeleted = null)
        {
            await GetSelectListItems();

            var incident = await Mediator.Send(new GetDetailedIncidentQuery { Id = id });

            var ambits = new List<SelectListItem>();
            if (incident.OriginId != null)
            {
                var ambitResponse = await Mediator.Send(new GetAmbitsByOriginQuery { OriginId = incident.OriginId.Value });
                ambits = SelectListMapper.GetSelectListFromDictionary(ambitResponse);
            }
            ViewBag.Ambits = ambits;

            var incidentTypes = new List<SelectListItem>();
            if (incident.AmbitId != null)
            {
                var incidentTypeResponse = await Mediator.Send
                    (new GetIncidentTypeByAmbitQuery { AmbitId = incident.AmbitId.Value });
                incidentTypes = SelectListMapper.GetSelectListFromDictionary(incidentTypeResponse);
            }
            ViewBag.IncidentTypes = incidentTypes;

            return View("DetailedIncident", incident);
        }

        [HttpGet]
        public async Task<ActionResult> GetCreateIncident(CreateIncidentViewModel request = null)
        {
            await GetSelectListItems();

            return View("CreateIncident", request);
        }

        [HttpGet]
        public async Task<ActionResult> GetAmbitsByOrigin(int? id)
        {
            var response = await Mediator.Send(new GetAmbitsByOriginQuery { OriginId = id ?? 0 });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public async Task<ActionResult> GetIncidentTypeByAmbit(int? id)
        {
            var response = await Mediator.Send(new GetIncidentTypeByAmbitQuery { AmbitId = id ?? 0 });
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public async Task<ActionResult> CreateIncident(CreateIncidentViewModel incident)
        {
            var validator = new CreateIncidentValidator();
            var validateResult = validator.Validate(incident);

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return await GetCreateIncident(incident);
            }

            await Mediator.Send(incident);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateIncident(int id)
        {
            await GetSelectListItems();

            var incident = await Mediator.Send(new GetUpdateIncidentQuery { Id = id });

            var ambits = new List<SelectListItem>();
            if(incident.OriginId != null)
            {
                var ambitResponse = await Mediator.Send
                    (new GetAmbitsByOriginQuery { OriginId = incident.OriginId.Value });
                ambits = SelectListMapper.GetSelectListFromDictionary(ambitResponse);
            }
            ViewBag.Ambits = ambits;

            var incidentTypes = new List<SelectListItem>();
            if (incident.AmbitId != null)
            {
                var incidentTypeResponse = await Mediator.Send
                    (new GetIncidentTypeByAmbitQuery { AmbitId = incident.AmbitId.Value });
                incidentTypes = SelectListMapper.GetSelectListFromDictionary(incidentTypeResponse);
            }
            ViewBag.IncidentTypes = incidentTypes;

            return View("UpdateIncident", incident);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateIncident(UpdateIncidentViewModel incident)
        {
            var validator = new UpdateIncidentValidator();
            var validateResult = validator.Validate(incident);

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return await GetUpdateIncident(incident.Id);
            }

            await Mediator.Send(incident);

            return RedirectToAction("Index");
        }

        private async Task GetSelectListItems()
        {
            ViewBag.Urgencies = SelectListMapper.GetSelectListFromEnum<Urgency>();

            ViewBag.Types = SelectListMapper.GetSelectListFromEnum<RequestType>();

            var threats = await Mediator.Send(new GetAllThreatsQuery());
            ViewBag.Threats = SelectListMapper.GetSelectListFromDictionary(threats);

            var scenarios = await Mediator.Send(new GetAllScenariosQuery());
            ViewBag.Scenarios = SelectListMapper.GetSelectListFromDictionary(scenarios);

            var origins = await Mediator.Send(new GetAllOriginsQuery());
            ViewBag.Origins = SelectListMapper.GetSelectListFromDictionary(origins);
        }
    }
}