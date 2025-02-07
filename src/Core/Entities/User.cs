using System;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;


namespace Core.Entities
{
    public class User : IdentityUser<int>
    {
        // public int Id { get; set; }
        public string FullName { get; set; } = null!;
        // public string Email { get; set; } = null!;
        // public string Role { get; set; } = null!;

        // Relationships
        public ICollection<Ticket> CreatedTickets { get; set; } = null!;
        public ICollection<Ticket> AssignedTickets { get; set; } = null!;
    }
}
