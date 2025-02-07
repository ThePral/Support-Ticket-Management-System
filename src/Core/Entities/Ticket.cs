using System;
using Core.Enums;

namespace Core.Entities
{
    public class Ticket
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public Priority Priority { get; set; }
        public Status Status { get; set; }
        public DepartmentType DepartmentType { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? ClosedAt { get; set; }

        // Relationships
        public int DepartmentId { get; set; }
        public Department Department { get; set; } = null!;

        public int CustomerId { get; set; }
        public User Customer { get; set; } = null!;

        public int? AgentId { get; set; }
        public User Agent { get; set; } = null!;

        public ICollection<Comment>? Comments { get; set; } // Comments on the ticket
    }
}