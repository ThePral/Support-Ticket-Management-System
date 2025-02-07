using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Application.DTOs;
using Core.Enums;

namespace Infrastructure.Services
{
    public interface ITicketService
    {
        Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto);
        Task<Ticket> UpdateTicketStatusAsync(int ticketId, Status status);
        Task<IEnumerable<Ticket>> GetTicketsByDepartmentAsync(int departmentId);
        Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId);
        Task<double> CalculateAverageResponseTimeAsync();
        Task<Dictionary<Status, int>> GetTicketsByStatusAsync();
        Task<Dictionary<Priority, int>> GetTicketsByPriorityAsync();
        Task<Dictionary<string, int>> GetTicketsResolvedPerDepartmentAsync();
        Task<TicketAttachmentDto> AddAttachmentAsync(TicketAttachmentDto attachmentDto);
        Task<IEnumerable<TicketAttachmentDto>> GetAttachmentsByTicketIdAsync(int ticketId);
    }
}
