using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.AspNetCore.Identity.Cognito;
using Amazon.Extensions.CognitoAuthentication;
using CognitoTest.DAL.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.Extensions.Logging;

namespace CognitoTest.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult index() => View();
    }

    public class AuthController : Controller
    {
        private readonly SignInManager<CognitoUser> _signInManager;
        private readonly CognitoUserManager<CognitoUser> _userManager;
        private readonly CognitoUserPool _pool;

        public AuthController(
            UserManager<CognitoUser> userManager,
            SignInManager<CognitoUser> signInManager,
            CognitoUserPool pool)
        {
            _userManager = userManager as CognitoUserManager<CognitoUser>;
            _signInManager = signInManager;
            _pool = pool;
        }

        [HttpGet]
        public IActionResult register() => View();

        [HttpGet]
        public IActionResult login() => View();

        [HttpPost]
        public async Task<IActionResult> register(RegisterViewModel model, string returnUrl = null)
        {
            if (this.ModelState.IsValid)
            {
                var user = _pool.GetUser(model.UserName);
                user.Attributes.Add(CognitoAttribute.Email.AttributeName, model.Email);
                user.Attributes.Add(CognitoAttribute.Name.AttributeName, model.Name);

                var result = _userManager.CreateAsync(user, model.Password).Result;
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "Secure");
                }
                //else
                    //return BadRequest($"<ul>{string.Concat(result.Errors.Select(ms => $"<li>{ms.Description}</li>"))}</ul>");
            }

            //return BadRequest($"<ul>{string.Concat(this.ModelState.Values.Where(ms => ms.Errors.Any()).Select(ms => $"<li>{ms.Errors[0].ErrorMessage}</li>"))}</ul>");
            return BadRequest();
        }

        [HttpPost]
        public IActionResult login(LoginViewModel model)
        {
            return BadRequest();
        }

        [HttpGet]
        public IActionResult logout() => RedirectToAction("index", "home");
    }

    [Authorize]
    public class SecureController : Controller
    {
        public IActionResult index() => View();

        [Authorize(Roles = "Admin")]
        public IActionResult admin() => View();
    }
}
