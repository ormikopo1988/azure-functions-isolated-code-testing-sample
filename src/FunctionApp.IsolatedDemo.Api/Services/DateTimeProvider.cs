using FunctionApp.IsolatedDemo.Api.Interfaces;
using System;

namespace FunctionApp.IsolatedDemo.Api.Services
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
