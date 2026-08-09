using Microsoft.AspNetCore.Mvc;
using SMS.API.Controllers.BaseAdminAuth;

namespace SMS.UI.Controllers.AdminDashbord
{
    public class AdminDashboardController : BaseAdminController
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
