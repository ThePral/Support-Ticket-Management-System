using System;
using Core.Enums;

namespace Core.Entities
{
    public class Agent
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DepartmentType Department { get; set; }

        // Number of tickets assigned (for workload tracking)
        public int AssignedTicketsCount { get; set; }

        // Navigation Property
        public List<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}