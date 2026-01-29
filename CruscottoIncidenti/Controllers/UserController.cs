using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Roles.Queries;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.User.Commands;
using CruscottoIncidenti.Application.User.Queries;
using CruscottoIncidenti.Application.Users.Validators;
using CruscottoIncidenti.Utils;
using FluentValidation;
using Microsoft.AspNet.Identity;
using static CruscottoIncidenti.Common.Constants;

namespace CruscottoIncidenti.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetUsersGrid(DataTablesParameters parameters)
        {
            var result = await Mediator.Send(new GetUsersGridQuery { Parameters = parameters });

            return new JsonCamelCaseResult(new
            {
                Draw = parameters.Draw,
                RecordsFiltered = result.Item1,
                RecordsTotal = result.Item1,
                Data = result.Item2
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetDetailedUser(int id, bool shouldBeDeleted = false)
        {
            var user = await Mediator.Send(new GetDetailedUserByIdQuery { Id = id });

            if (user != null)
                user.ShouldBeDeleted = shouldBeDeleted;

            //var selectRoles = new List<SelectListItem>();
            var userRoles = await Mediator.Send(new GetRolesQuery());

            var selectRoles = new List<SelectListItem>();
            foreach (var role in userRoles)
            {
                selectRoles.Add(new SelectListItem() 
                { 
                    Value = role.Id.ToString(), 
                    Text = role.Name 
                });
            }

            ViewBag.AllRoles = selectRoles;

            //user.UserRoles = user.AllRoles.Where(x => x.IsSelected).Select(x => x.Id).ToList();

            return PartialView("_DetailedUserModal", user);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            HttpStatusCode statusCode = await Mediator.Send(new DeleteUserCommand { Id = id }) ?
                HttpStatusCode.OK :
                HttpStatusCode.InternalServerError;

            return new HttpStatusCodeResult(statusCode);
        }

        [HttpGet]
        public async Task<ActionResult> GetCreateUser(CreateUserCommand user = null)
        {
            var roles = await Mediator.Send(new GetRolesQuery());

            var selectRoles = new List<SelectListItem>();
            foreach (var role in roles)
            {
                selectRoles.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Name });
            }

            ViewBag.AllRoles = selectRoles;

            return PartialView("_CreateUserModal", user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateUser(CreateUserCommand user)
        {
            user.CreatorId = int.Parse(HttpContext.User.Identity.GetUserId());

            var validator = new CreateUserValidator();
            var validateResult = validator.Validate(user);

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            var passwordValidateResult = validator.Validate(user, ruleSet: PasswordRuleSet);
            ModelState.ValidatePasswordCharRules(passwordValidateResult, user);

            if (!validateResult.IsValid || !passwordValidateResult.IsValid)
                return await GetCreateUser(user);

            HttpStatusCode statusCode;
            try
            {
                statusCode = await Mediator.Send(user) ?
                    HttpStatusCode.OK : 
                    HttpStatusCode.InternalServerError;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("IncorrectPostUser", ex.FriendlyMessage);
                return await GetCreateUser(user);
            }

            return new HttpStatusCodeResult(statusCode);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateUser(int id)
        {
            var user = await Mediator.Send(new GetUserByIdQuery() { Id = id });

            if(user == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            var selectRoles = new List<SelectListItem>();
            foreach (var role in user.Roles)
            {
                selectRoles.Add(new SelectListItem() 
                { 
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }

            ViewBag.AllRoles = selectRoles;
            var viewUser = Mapper.Map<UpdateUserCommand>(user);

            if (ModelState.ContainsKey("Roles"))
                viewUser.Roles = new List<int>();

            return PartialView("_UpdateUserModal", viewUser);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UpdateUserCommand user)
        {
            user.EditorId = int.Parse(HttpContext.User.Identity.GetUserId());

            var validator = new UpdateUserValidation();
            var validateResult = validator.Validate(user);

            if (!validateResult.IsValid)
            {
                foreach (var error in validateResult.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }

            var passwordValidateResult = validator.Validate(user, ruleSet: PasswordRuleSet);
            ModelState.ValidatePasswordCharRules(passwordValidateResult, user);

            if (!validateResult.IsValid || !passwordValidateResult.IsValid)
                return await GetUpdateUser(user.UserId);

            HttpStatusCode statusCode;
            try
            {
                statusCode = await Mediator.Send(user) ? 
                    HttpStatusCode.OK : 
                    HttpStatusCode.InternalServerError;
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("IncorrectUpdateUser", ex.FriendlyMessage);
                return await GetUpdateUser(user.UserId);
            }

            return new HttpStatusCodeResult(statusCode);
        }
    }
}