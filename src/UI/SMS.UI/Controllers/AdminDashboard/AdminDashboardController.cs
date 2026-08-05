using Microsoft.AspNetCore.Mvc;

namespace SMS.UI.Controllers.AdminDashbord
{
    public class AdminDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Dashboard()
        {
            // TODO: real data yahan se lao — employeeManager.Get(), stats calculate karo, etc.
            return View();
        }
    }
}
