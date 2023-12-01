using System;

namespace FunctionApp.IsolatedDemo.Api.Entities
{
    public abstract class AuditableEntity : BaseEntity
    {
        public DateTime CreatedAt { get; set; }

        public DateTime LastUpdatedOn { get; set; }
    }
}
