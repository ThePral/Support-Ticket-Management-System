using System;

namespace Core.Interfaces
{
    public interface ITicketAssignmentService
    {
        Task<bool> AssignTicketAsync(int ticketId);
        
    }
}