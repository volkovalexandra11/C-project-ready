﻿namespace WebApplication2.Models
{
    public class ErrorModel
    {
        public ErrorModel(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}