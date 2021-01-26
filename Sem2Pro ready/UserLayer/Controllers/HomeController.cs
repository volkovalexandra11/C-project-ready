using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {

        public IActionResult ByFunction()
        {
            return View(new ByFunctionPageViewModel
            {
                Functions = new []
                {
                    FunctionDescription.Default
                }
            });
        }

        public IActionResult ByPoints()
        {
            return View(new ByPointsPageViewModel
            {
                Functions = new []
                {
                    PointsDescription.Default
                }
            });
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Hello! \n I'm a very stupid graph drawer";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
