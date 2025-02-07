using System;
using Core.Entities;

namespace Core.Interfaces
{
    public interface ITicketRepository
    {
        Task<Ticket?> GetByIdAsync(int id);
        Task UpdateAsync(Ticket ticket);
        Task AddAsync(Ticket ticket);
        Task<IEnumerable<Ticket>> GetTicketsByAgentIdAsync(int agentId);
        Task<bool> DeleteAsync(int id);
    }
}