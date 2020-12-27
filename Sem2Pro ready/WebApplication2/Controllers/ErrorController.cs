using Microsoft.AspNetCore.Mvc;

namespace UserLayer.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Error()
        {
            return View();
        }
    }
}