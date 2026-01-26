using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using CruscottoIncidenti.Application.User.Queries.GetUserByUserName;
using CruscottoIncidenti.Application.Users.Validators;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;

namespace CruscottoIncidenti.Controllers
{
    public class AccountController : BaseController
    {
        public ActionResult Login() => View("Login");

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(string username, string password)
        {
            try
            {
                var query = new GetUserByUserNameQuery { UserName = username, Password = password };

                var validator = new GetUserByUsernameValidator();
                var validationResult = validator.Validate(query);

                if(!validationResult.IsValid)
                {
                    foreach(var error in validationResult.Errors)
                    {
                        ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                    }

                    return View("Login");
                }

                var userModel = await Mediator.Send(query);

                if (userModel == null || !userModel.IsEnabled)
                {
                    ModelState.AddModelError("IncorrectLogin", "Non-existent or disabled user");
                    return View("Login");
                }
                else
                {
                    var userClaims = new List<Claim>
                    {
                        new Claim(ClaimTypes.NameIdentifier, userModel.Id.ToString()),
                        new Claim(ClaimTypes.Name, userModel.Username),
                        new Claim("FullName", userModel.FullName),
                        new Claim(ClaimTypes.Email, userModel.Email)
                    };

                    foreach (var role in userModel.Roles)
                        userClaims.Add(new Claim(ClaimTypes.Role, role.Name));

                    var claimsIdentity = new ClaimsIdentity(DefaultAuthenticationTypes.ApplicationCookie);
                    claimsIdentity.AddClaims(userClaims);

                    var principal = new ClaimsPrincipal(claimsIdentity);

                    var context = Request.GetOwinContext();
                    var authManager = context.Authentication;
                    authManager.SignIn(new AuthenticationProperties { IsPersistent = false }, claimsIdentity);

                    return RedirectToAction(nameof(HomeController.Index), "Home");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return View("Login");
            }
        }

        [Authorize]
        [HttpGet]
        public ActionResult Logout()
        {
            var context = Request.GetOwinContext();
            var authManager = context.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction(nameof(Login), "Account");
        }
    }
}