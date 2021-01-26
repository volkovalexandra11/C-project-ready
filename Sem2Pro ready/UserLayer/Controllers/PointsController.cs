using System;
using System.Collections.Generic;
using System.Linq;
using Infrastructure.TopDowns;
using Microsoft.AspNetCore.Mvc;
using UserLayer.Controllers.Auxiliary;
using WebApplication2.Models;

namespace UserLayer.Controllers
{
    public class PointsController : Controller
    {
        private readonly Cache cache;
        private readonly PointsDrawingService drawer;


        public PointsController(PointsDrawingService drawer, Cache cache)
        {
            this.drawer = drawer;
            this.cache = cache;
        }

        public IActionResult ProcessAdd(PointsDescription[] pointsDescriptions)
        {
            var model = new ByPointsPageViewModel()
            {
                Functions = pointsDescriptions.Append(PointsDescription.Default).ToArray()
            };

            return View("../Home/ByPoints", model);
        }

        public IActionResult ProcessDraw(PointsDescription[] pointsDescriptions)
        {
            try
            {
                return View("Graph", drawer.Draw(pointsDescriptions));
            }
            catch (DrawingException e)
            {
                return Error(e.Message);
            }
        }

        public IActionResult NewGraph(Guid name)
        {
            cache.TryGet(name, out var content);
            return File(content, "image/jpeg");
        }

        public IActionResult Error(string message)
        {
            return View("../Graph/Error", new ErrorModel(message));
        }
    }
}