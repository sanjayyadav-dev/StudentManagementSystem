using Microsoft.AspNetCore.Mvc;

namespace SMS.UI.Controllers
{
    public class AboutUs : Controller
    {
        public IActionResult About()
        {
            return View();
        }
    }
}
