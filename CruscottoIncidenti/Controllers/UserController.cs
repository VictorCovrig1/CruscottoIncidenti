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
using CruscottoIncidenti.Application.Users.ViewModels;
using CruscottoIncidenti.Common;
using CruscottoIncidenti.Filters;
using CruscottoIncidenti.Utils;
using FluentValidation;
using static CruscottoIncidenti.Common.Constants;

namespace CruscottoIncidenti.Controllers
{
    [ClaimsAuthorize(Role.Administrator)]
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetUsersGrid(DataTablesParameters parameters)
        {
            var result = await Mediator.Send(new GetUsersGridQuery { Parameters = parameters });

            return Json(new
            {
                draw = parameters.Draw,
                recordsFiltered = parameters.TotalCount,
                recordsTotal = parameters.TotalCount,
                data = result
            });
        }

        [HttpGet]
        public async Task<ActionResult> GetDetailedUser(int id, bool shouldBeDeleted = false)
        {
            var user = await Mediator.Send(new GetDetailedUserByIdQuery { Id = id });
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

            return PartialView("_DetailedUserModal", user);
        }

        [HttpPost]
        public async Task<ActionResult> DeleteUser(int id)
        {
            try
            {
                await Mediator.Send(new DeleteUserCommand { Id = id });
            }
            catch(CustomException ex)
            {
                ModelState.AddModelError("IncorrectDeleteUser", ex.FriendlyMessage);
                return await GetDetailedUser(id, true);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<ActionResult> GetCreateUser(CreateUserViewModel user = null)
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
        public async Task<ActionResult> CreateUser(CreateUserViewModel user)
        {
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

            try
            {
                await Mediator.Send(user);
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("IncorrectPostUser", ex.FriendlyMessage);
                return await GetCreateUser(user);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpGet]
        public async Task<ActionResult> GetUpdateUser(int id)
        {
            var user = await Mediator.Send(new GetUpdateUserQuery() { Id = id });

            if(user == null)
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);

            var roles = await Mediator.Send(new GetRolesQuery());

            var selectRoles = new List<SelectListItem>();
            foreach (var role in roles)
            {
                selectRoles.Add(new SelectListItem() 
                { 
                    Value = role.Id.ToString(),
                    Text = role.Name
                });
            }

            ViewBag.AllRoles = selectRoles;

            return PartialView("_UpdateUserModal", user);
        }

        [HttpPost]
        public async Task<ActionResult> UpdateUser(UpdateUserViewModel user)
        {
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
                return await GetUpdateUser(user.Id);

            try
            {
                await Mediator.Send(user); 
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError("IncorrectUpdateUser", ex.FriendlyMessage);
                return await GetUpdateUser(user.Id);
            }

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}