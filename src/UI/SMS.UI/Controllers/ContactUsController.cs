using Microsoft.AspNetCore.Mvc;
using SMS.UI.Models;

namespace SMS.UI.Controllers
{
    public class ContactUsController : Controller
    {
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ContactUs(ContactFormModel model)
        {
            return Json(new { success = true, message = "Thank you for contacting us!" });
        }
    }
}
