using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Globalization;
using System.IO;
using System.Linq;
using FunctionGraph;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.TopDowns;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        private static readonly List<string> function = new List<string>();
        private static readonly List<Color> color = new List<Color>();
        private static FunctionDrawer fDrawer;
        private static Email email;
        private static ParserCombinator parser;
        private static readonly List<int> leftBorders = new List<int>();
        private static readonly List<int> rightBorders = new List<int>();
        private static bool sent;

        public HomeController(FunctionDrawer functionDrawer, Email email, ParserCombinator parserComb)
        {
            fDrawer = functionDrawer;
            HomeController.email = email;
            parser = parserComb;
        }

        // public IActionResult Index(int functionCount=0)
        // {
        //     @ViewBag.Functions = function;
        //     @ViewBag.Colors = color;
        //     @ViewBag.FunctionCount = functionCount + 1;
        //     return View();
        // }
        
        public IActionResult Index()
        {
            if (sent)
            {
                function.Clear();
                color.Clear();
                leftBorders.Clear();
                rightBorders.Clear();
            }
            return View();
        }
        
        [HttpPost]
        public IActionResult Graph(string func, 
            Color col, 
            int leftBorder, 
            int rightBorder, 
            string draw,
            string addFunction)
        {
            function.Add(func);
            color.Add(col);
            leftBorders.Add(leftBorder);
            rightBorders.Add(rightBorder);
            ViewBag.Function = func;
            if (!string.IsNullOrEmpty(draw))
            {
                return View();
            }
            if (!string.IsNullOrEmpty(addFunction))
            {
                return RedirectToAction("Index");
            }
            return View("Index");
        }


        public IActionResult Send()
        {
            return View();
        }

        public IActionResult SendAll()
        {
            return RedirectToAction("Index");
        }

        public IActionResult SendMail(string email1)
        {
            sent = true;
            var funcsToDraw = function.Select(f => parser.Parse(f)).ToArray();
            var picture = DrawGraph(funcsToDraw);
            var graph = Draw(picture);
            email.SendEmail(graph, email1);
            return RedirectToAction("Index");
        }
        
        public IActionResult NewGraph()
        {
            var funcsToDraw = function.Select(f => parser.Parse(f)).ToArray();
            var picture = DrawGraph(funcsToDraw);
            var graph = Draw(picture);
            return File(graph, "image/jpeg");
        }

        public IActionResult Test()
        {
            return View();
        }

        public Bitmap DrawGraph(Func<double, double>[] functions)
        {
            var funcs = functions
                .Select((t, i) => new Function(t, color[i], function[i], leftBorders[i], rightBorders[i]))
                .ToList();
            var picture = fDrawer.CreateChart(funcs.ToArray());
            return picture;
        }

        public byte[] Draw(Bitmap picture)
        {
            using (var stream = new MemoryStream())
            {
                picture.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
        }
        
        public IActionResult Save()
        {
            var funcsToDraw = function.Select(f => parser.Parse(f)).ToArray();
            var picture = DrawGraph(funcsToDraw);
            var graph = Draw(picture);
            var content = new MemoryStream(graph);
            var contentType = "APPLICATION/octet-stream";
            var name = $"{DateTime.Now.ToString(CultureInfo.InvariantCulture).Replace('/', ' ')}.jpeg";
            return File(content, contentType, name);
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
