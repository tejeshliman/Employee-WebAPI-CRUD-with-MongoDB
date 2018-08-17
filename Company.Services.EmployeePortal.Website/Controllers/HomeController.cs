using System.Web.Mvc;

namespace EmployeeApp.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public RedirectToRouteResult Index()
        {
            ViewBag.Title = "Home Page";

            return RedirectToAction("Index");
        }
    }
}
