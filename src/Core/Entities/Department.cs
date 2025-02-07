using System;

namespace Core.Entities
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        // Relationships
        public ICollection<Ticket> Tickets { get; set; } = null!;
    }
}