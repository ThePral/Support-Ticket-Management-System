using System;
using Infrastructure.Data;
using Core.Entities;
using Core.Enums;
using Core.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly AppDbContext _context;

        public AgentRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Agent?> GetAvailableAgentAsync(DepartmentType department)
        {
            return await _context.Agents
                .Where(a => a.Department == department)
                .OrderBy(a => a.AssignedTicketsCount)  // Least busy agent first
                .FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(Agent agent)
        {
            _context.Agents.Update(agent);
            await _context.SaveChangesAsync();
        }
    }

}