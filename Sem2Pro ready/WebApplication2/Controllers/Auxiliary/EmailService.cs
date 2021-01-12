using System;
using WebApplication2.Controllers;

namespace UserLayer.Controllers.Auxiliary
{
    public class EmailService
    {
        private readonly Cache cache;
        private readonly Email email;

        public EmailService(Cache cache, Email email)
        {
            this.cache = cache;
            this.email = email;
        }

        public void Send(string email1, Guid name)
        {
            cache.TryGet(name, out var graph);
            email.SendEmail(graph, email1);
        }
    }
}