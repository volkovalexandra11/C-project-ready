using System;
using Microsoft.AspNetCore.Mvc;
using UserLayer.Controllers.Auxiliary;
using WebApplication2.Controllers;

namespace UserLayer.Controllers
{
    public class EmailController : Controller
    {
        private readonly EmailService email;
        public EmailController(EmailService email)
        {
            this.email = email;
        }

        public IActionResult SendMail(string email1, Guid name)
        {
            email.Send(email1, name);
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Send(Guid name)
        {
            return View("../Home/Send", name);
        }

        public IActionResult SendAll()
        {
            return RedirectToAction("Index", "Home");
        }
    }
}