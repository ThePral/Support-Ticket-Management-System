using System;
using Core.Entities;
using Core.Enums;

namespace Core.Interfaces
{
    public interface IAgentRepository
    {
        Task<Agent?> GetAvailableAgentAsync(DepartmentType department);
        Task UpdateAsync(Agent agent);
    }
}