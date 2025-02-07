using System;
using Core.Enums;

namespace Application.DTOs
{
    public class CreateTicketDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Priority Priority { get; set; }
        public int CustomerId { get; set; }
        public int DepartmentId { get; set; }
    }
}