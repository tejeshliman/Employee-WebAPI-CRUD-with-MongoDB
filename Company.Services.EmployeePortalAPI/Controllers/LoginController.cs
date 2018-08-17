using Company.Services.EmployeePortalAPI.Models;
using Microsoft.Owin.Security;
using System.Web;
using System.Web.Mvc;

namespace Company.Services.EmployeePortalAPI.Controllers
{
    public class LoginController : Controller
    {
        //
        // GET: /Account/Login
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public bool Login(string email, string password)
        {
            IAuthenticationManager authenticationManager = HttpContext.GetOwinContext().Authentication;
            var authService = new AdAuthenticationService(authenticationManager);

            var authenticationResult = authService.SignIn(email, password);

            if (authenticationResult.IsSuccess)
            {
                // we are in!
                return true;
            }
            return false;
        }

    }
}
