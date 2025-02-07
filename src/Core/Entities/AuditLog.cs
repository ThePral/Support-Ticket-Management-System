using System;
using Core.Enums;

namespace Core.Entities
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!; //Who performed the action?
        public string UserRole { get; set; } = null!; // What role did they have?
        public string Action { get; set; } = null!; // What was the action?
        public string Entity { get; set; } = null!; // What entity was affected?
        public int EntityId { get; set; } // ID of the affected entity
        public DateTime Timestamp { get; set; } = DateTime.UtcNow; // When?
        public string IPAddress { get; set; } = null!; // From where?
    }
}