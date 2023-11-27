using System;

namespace FunctionApp.IsolatedDemo.Api.Interfaces
{
    public interface IDateTimeProvider
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}
