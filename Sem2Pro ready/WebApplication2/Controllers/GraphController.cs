﻿using System;
using System.Collections.Generic;
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

        [HttpPost]
        public IActionResult Graph(FunctionDescription[] functionDescriptions,
            string draw,
            string addFunctions)
        {
            if (!string.IsNullOrEmpty(draw))
            {
                return ProcessDraw(functionDescriptions);
            }
            
            if (!string.IsNullOrEmpty(addFunctions))
            {
                var model = new ByFunctionPageViewModel
                {
                    Functions = functionDescriptions.Append(new FunctionDescription
                    {
                        Function = "",
                        LeftBorder = -10,
                        RightBorder = 10
                    }).ToArray()
                };

                return View("../Home/ByFunction", model);
            }

            return RedirectToAction("ByFunction", "Home");
        }

        private IActionResult ProcessDraw(FunctionDescription[] functionDescriptions)
        {
            try
            {
                return View(drawer.Draw(functionDescriptions));
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

        public IActionResult Save(Guid name)
        {
            const string contentType = "APPLICATION/octet-stream";
            var fileDownloadName = $"{DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', ' ')}.jpeg";
            return File(cache.Get(name), contentType, fileDownloadName);
        }

        public IActionResult Error(string message)
        {
            return View("Error", new ErrorModel(message));
        }
    }
}