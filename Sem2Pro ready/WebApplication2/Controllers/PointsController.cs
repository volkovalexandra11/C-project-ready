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


        public IActionResult Graph(PointsDescription[] pointsDescriptions, string draw, string addFunctions)
        {
            if (!string.IsNullOrEmpty(draw))
            {
                return ProcessDraw(pointsDescriptions);
            }

            if (!string.IsNullOrEmpty(addFunctions))
            {
                var model = new ByPointsPageViewModel()
                {
                    Functions = pointsDescriptions.Append(new PointsDescription()
                    {
                        Points = ""
                    }).ToArray()
                };

                return View("../Home/ByPoints", model);
            }
            return View();
        }

        private IActionResult ProcessDraw(PointsDescription[] pointsDescriptions)
        {
            try
            {
                return View(drawer.Draw(pointsDescriptions));
            }
            catch (DrawingException e)
            {
                return Error(e.Message);
            }
        }

        public IActionResult NewGraph(Guid name)
        {
            return File(cache.Get(name), "image/jpeg");
        }

        public IActionResult Error(string message)
        {
            return View("../Graph/Error", new ErrorModel(message));
        }
    }
}