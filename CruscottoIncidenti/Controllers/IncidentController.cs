using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Incidents.Commands;
using CruscottoIncidenti.Application.Incidents.Queries.Ambits;
using CruscottoIncidenti.Application.Incidents.Queries.Incidents;
using CruscottoIncidenti.Application.Incidents.Queries.IncidentTypes;
using CruscottoIncidenti.Application.Incidents.Queries.Origins;
using CruscottoIncidenti.Application.Incidents.Queries.Scenarios;
using CruscottoIncidenti.Application.Incidents.Queries.Threats;
using CruscottoIncidenti.Application.Incidents.Validators;
using CruscottoIncidenti.Application.Incidents.ViewModels;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Filters;
using CruscottoIncidenti.Utils;

namespace CruscottoIncidenti.Controllers
{
    [ClaimsAuthorize(Role.Operator, Role.User)]
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
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = result
            });
        }

        [HttpGet]
        [ClaimsAuthorize(Role.Operator)]
        public async Task<ActionResult> GetDetailedIncident(int id, bool shouldBeDeleted = false)
        {
            var incident = await Mediator.Send(new GetDetailedIncidentQuery { Id = id });

            if(incident == null)
                return RedirectToAction(nameof(ErrorController.NotFound), "Error");

            ViewBag.ShouldBeDeleted = shouldBeDeleted;

            return View("DetailedIncident", incident);
        }

        [HttpGet]
        public async Task<ActionResult> GetCreateIncident(CreateIncidentViewModel request)
        {
            await GetSelectListItems(request.OriginId, request.AmbitId);
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
            try
            {
                await Mediator.Send(incident);
            }
            catch(CustomException ex)
            {
                ModelState.AddModelError("IncorrectPostIncident", ex.FriendlyMessage);
                return await GetCreateIncident(incident);
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        [ClaimsAuthorize(Role.Operator)]
        public async Task<ActionResult> GetUpdateIncident(int id)
        {
            var incident = await Mediator.Send(new GetUpdateIncidentQuery { Id = id });

            if (incident == null)
                return RedirectToAction(nameof(ErrorController.NotFound), "Error");

            await GetSelectListItems(incident.OriginId, incident.AmbitId);

            return View("UpdateIncident", incident);
        }

        [HttpPost]
        [ClaimsAuthorize(Role.Operator)]
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

            try
            {
                await Mediator.Send(incident);
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("IncorrectUpdateIncident", ex.FriendlyMessage);
                return await GetUpdateIncident(incident.Id);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ClaimsAuthorize(Role.Operator)]
        public async Task<ActionResult> DeleteIncident(int id)
        {
            try
            {
                await Mediator.Send(new DeleteIncidentCommand { Id = id });
            }
            catch(CustomException ex)
            {
                ModelState.AddModelError("IncorrectDeleteIncident", ex.FriendlyMessage);
                return await GetDetailedIncident(id, true);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ClaimsAuthorize(Role.Operator)]
        public async Task<ActionResult> ImportIncidents(ImportIncidentsViewModel incidents)
        {
            //var validator = new ImportIncidentsValidator();
            //var validateResult = await validator.ValidateAsync(incidents);

            //if (!validateResult.IsValid)
            //{
            //    var invalidIndices = validateResult.Errors
            //        .Select(e => System.Text.RegularExpressions.Regex.Match(e.PropertyName, @"\[(\d+)\]"))
            //        .Where(m => m.Success)
            //        .Select(m => int.Parse(m.Groups[1].Value))
            //        .Distinct();

            //    foreach (var index in invalidIndices)
            //    {
            //        incidents.Incidents.RemoveAt(index);
            //    }
            //}

            var insertedIncidents = await Mediator.Send(incidents);

            return Json(insertedIncidents);
        }

        private async Task GetSelectListItems(int? originId, int? ambitId)
        {
            ViewBag.Urgencies = SelectListMapper.GetSelectListFromEnum<Urgency>();

            ViewBag.Types = SelectListMapper.GetSelectListFromEnum<RequestType>();

            var threats = await Mediator.Send(new GetAllThreatsQuery());
            ViewBag.Threats = SelectListMapper.GetSelectListFromDictionary(threats);

            var scenarios = await Mediator.Send(new GetAllScenariosQuery());
            ViewBag.Scenarios = SelectListMapper.GetSelectListFromDictionary(scenarios);

            var origins = await Mediator.Send(new GetAllOriginsQuery());
            ViewBag.Origins = SelectListMapper.GetSelectListFromDictionary(origins);

            var ambits = new List<SelectListItem>();
            if (originId != null)
            {
                var ambitResponse = await Mediator.Send
                    (new GetAmbitsByOriginQuery { OriginId = originId.Value });
                ambits = SelectListMapper.GetSelectListFromDictionary(ambitResponse);
            }
            ViewBag.Ambits = ambits;

            var incidentTypes = new List<SelectListItem>();
            if (ambitId != null)
            {
                var incidentTypeResponse = await Mediator.Send
                    (new GetIncidentTypeByAmbitQuery { AmbitId = ambitId.Value });
                incidentTypes = SelectListMapper.GetSelectListFromDictionary(incidentTypeResponse);
            }
            ViewBag.IncidentTypes = incidentTypes;
        }
    }
}