using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Ambits.Queries;
using CruscottoIncidenti.Application.Incidents.Commands.CreateIncident;
using CruscottoIncidenti.Application.Incidents.Queries;
using CruscottoIncidenti.Application.Incidents.Validators;
using CruscottoIncidenti.Application.IncidentTypes.Queries;
using CruscottoIncidenti.Application.Origins.Queries;
using CruscottoIncidenti.Application.Scenarios.Queries;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.Threats.Queries;
using CruscottoIncidenti.Common;
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

        [HttpGet]
        public async Task<ActionResult> GetCreateIncident(CreateIncidentCommand request = null)
        {
            var selectUrgencies = new List<SelectListItem>();
            var urgencies = Enum.GetValues(typeof(Urgency)).Cast<Urgency>()
                .ToDictionary(k => (int)k, v => v.ToString());

            foreach (var urgency in urgencies)
            {
                selectUrgencies.Add(new SelectListItem() { Value = urgency.Key.ToString(), Text = urgency.Value });
            }

            ViewBag.Urgencies = selectUrgencies;

            var selectTypes = new List<SelectListItem>();
            var types = Enum.GetValues(typeof(RequestType)).Cast<RequestType>()
                .ToDictionary(k => (int)k, v => v.ToString());

            foreach (var type in types)
            {
                selectTypes.Add(new SelectListItem() { Value = type.Key.ToString(), Text = type.Value });
            }

            ViewBag.Types = selectTypes;

            var threats = await Mediator.Send(new GetAllThreatsQuery());
            var selectThreats = new List<SelectListItem>();

            foreach (var threat in threats)
            {
                selectThreats.Add(new SelectListItem() { Value = threat.Key.ToString(), Text = threat.Value });
            }

            ViewBag.Threats = selectThreats;

            var scenarios = await Mediator.Send(new GetAllScenariosQuery());
            var selectScenarios = new List<SelectListItem>();

            foreach (var scenario in scenarios)
            {
                selectScenarios.Add(new SelectListItem() { Value = scenario.Key.ToString(), Text = scenario.Value });
            }

            ViewBag.Scenarios = selectScenarios;

            var origins = await Mediator.Send(new GetAllOriginsQuery());
            var selectOrigins = new List<SelectListItem>();

            foreach (var origin in origins)
            {
                selectOrigins.Add(new SelectListItem() { Value = origin.Key.ToString(), Text = origin.Value });
            }

            ViewBag.Origins = selectOrigins;

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

                return await GetCreateIncident(incident);
            }

            try
            {
                var isSuccessful = await Mediator.Send(incident);

                if (isSuccessful)
                    return RedirectToAction("Index");
                else
                {
                    ModelState.AddModelError("IncorrectPostIncident", "An unexpected exception occured");
                    return await GetCreateIncident(incident);
                }  
            }
            catch(Exception)
            {
                ModelState.AddModelError("IncorrectPostIncident", "An unexpected exception occured");
                return await GetCreateIncident(incident);
            }
        }
    }
}