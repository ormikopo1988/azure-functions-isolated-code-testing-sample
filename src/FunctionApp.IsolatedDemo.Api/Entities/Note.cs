﻿namespace FunctionApp.IsolatedDemo.Api.Entities
{
    public class Note : AuditableEntity
    {
        public string Title { get; set; } = default!;

        public string Body { get; set; } = default!;
    }
}
