using System;

namespace FunctionApp.IsolatedDemo.Api.Entities
{
    public abstract class BaseEntity
    {
        public Guid Id { get; set; }
    }
}
