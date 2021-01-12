using System;
using System.Globalization;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UserLayer.Controllers.Auxiliary;
using WebApplication2.Models;

namespace UserLayer.Controllers
{
    public class GraphController : Controller
    {
        private readonly DrawingService drawer;
        private readonly Cache cache;

        public GraphController(DrawingService drawer, Cache cache)
        {
            this.drawer = drawer;
            this.cache = cache;
        }

        public IActionResult ProcessDraw(FunctionDescription[] functionDescriptions)
        {
            try
            {
                return View("Graph", drawer.Draw(functionDescriptions));
            }
            catch (DrawingException e)
            {
                return Error(e.Message);
            }
        }

        public IActionResult ProcessAdd(FunctionDescription[] functionDescriptions)
        {
            var model = new ByFunctionPageViewModel
            {
                Functions = functionDescriptions.Append(FunctionDescription.Default).ToArray()
            };

            return View("../Home/ByFunction", model);
        }
        
        public IActionResult NewGraph(Guid name)
        {
            cache.TryGet(name, out var content);
            return File(content, "image/jpeg");
        }

        public IActionResult Save(Guid name)
        {
            const string contentType = "APPLICATION/octet-stream";
            var fileDownloadName = $"{DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', ' ')}.jpeg";
            cache.TryGet(name, out var content);
            return File(content, contentType, fileDownloadName);
        }

        private IActionResult Error(string message)
        {
            return View("Error", new ErrorModel(message));
        }
    }
}