using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Mvc;
using CruscottoIncidenti.Application.Common.Exceptions;
using CruscottoIncidenti.Application.Roles.Queries.GetRoles;
using CruscottoIncidenti.Application.TableParameters;
using CruscottoIncidenti.Application.User.Commands.CreateUser;
using CruscottoIncidenti.Application.User.Commands.CreateUser.Validation;
using CruscottoIncidenti.Application.User.Commands.UpdateUser;
using CruscottoIncidenti.Application.User.Commands.UpdateUser.Validation;
using CruscottoIncidenti.Application.User.Queries.GetUserById;
using CruscottoIncidenti.Application.User.Queries.GetUsers;
using CruscottoIncidenti.Utils;
using Microsoft.AspNet.Identity;

namespace CruscottoIncidenti.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class UserController : BaseController
    {
        [HttpGet]
        public ActionResult Index() => View();

        [HttpPost]
        public async Task<ActionResult> GetUserGrid(DataTablesParameters parameters)
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
        public async Task<ActionResult> GetCreateUserModal(CreateUserCommand user = null)
        {
            var roles = await Mediator.Send(new GetRolesQuery());

            var selectRoles = new List<SelectListItem>();
            foreach (var role in roles)
            {
                selectRoles.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Name });
            }

            ViewBag.Roles = selectRoles;

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

                return await GetCreateUserModal(user);
            }

            HttpStatusCode statusCode;
            try
            {
                statusCode = await Mediator.Send(user) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                if (statusCode == HttpStatusCode.BadRequest)
                    throw new Exception("Could not insert user");
            }
            catch (DublicatedEntityException ex)
            {
                ModelState.AddModelError("IncorrectPostUser", ex.Message);
                return await GetCreateUserModal(user);
            }
            catch(Exception)
            {
                ModelState.AddModelError("IncorrectPostUser", "Unexpected error occured");
                return await GetCreateUserModal(user);
            }

            return new HttpStatusCodeResult(statusCode);
        }

        [HttpGet]
        public async Task<ActionResult> GetEditUserModal(int id)
        {
            var user = await Mediator.Send(new GetUserByIdQuery() { Id = id });

            var selectRoles = new List<SelectListItem>();
            foreach (var role in user.Roles)
            {
                selectRoles.Add(new SelectListItem() { Value = role.Id.ToString(), Text = role.Name });
            }

            ViewBag.Roles = selectRoles;
            var viewUser = Mapper.Map<UpdateUserCommand>(user);

            return PartialView("_UpdateUserModal", viewUser);
        }

        [HttpPost]
        public async Task<ActionResult> EditUser(UpdateUserCommand user)
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

                return await GetEditUserModal(user.UserId);
            }

            HttpStatusCode statusCode;
            try
            {
                statusCode = await Mediator.Send(user) ? HttpStatusCode.OK : HttpStatusCode.BadRequest;

                if (statusCode == HttpStatusCode.BadRequest)
                    throw new Exception("Could not update user");
            }
            catch (DublicatedEntityException ex)
            {
                ModelState.AddModelError("IncorrectUpdateUser", ex.Message);
                return await GetEditUserModal(user.UserId);
            }
            catch (Exception)
            {
                ModelState.AddModelError("IncorrectPostUser", "Unexpected error occured");
                return await GetEditUserModal(user.UserId);
            }

            return new HttpStatusCodeResult(statusCode);
        }
    }
}