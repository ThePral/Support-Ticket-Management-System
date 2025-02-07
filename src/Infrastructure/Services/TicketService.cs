using System;
using Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Data;
using Core.Entities;
using Core.Enums;

namespace Infrastructure.Services
{
    public class TicketService : ITicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Ticket> CreateTicketAsync(CreateTicketDto ticketDto)
        {
            var ticket = new Ticket
            {
                Title = ticketDto.Title,
                Description = ticketDto.Description,
                Priority = ticketDto.Priority,
                Status = Status.New,
                CreatedAt = DateTime.UtcNow,
                CustomerId = ticketDto.CustomerId,
                DepartmentId = ticketDto.DepartmentId
            };

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();

            return ticket;
        }

        public async Task<Ticket> UpdateTicketStatusAsync(int ticketId, Status status)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket == null) throw new Exception("Ticket not found");

            ticket.Status = status;
            ticket.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return ticket;
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByDepartmentAsync(int departmentId)
        {
            return await _context.Tickets
                .Where(t => t.DepartmentId == departmentId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Ticket>> GetTicketsByUserAsync(int userId)
        {
            return await _context.Tickets
                .Where(t => t.CustomerId == userId || t.AgentId == userId)
                .ToListAsync();
        }

        public async Task<double> CalculateAverageResponseTimeAsync()
        {
            var tickets = await _context.Tickets
                .Where(t => t.Status == Status.Resolved || t.Status == Status.Closed)
                .ToListAsync();

            if (!tickets.Any()) return 0;

            var totalResponseTime = tickets
                .Where(t => t.UpdatedAt.HasValue)
                .Sum(t => (t.UpdatedAt!.Value - t.CreatedAt).TotalMinutes);

            return totalResponseTime / tickets.Count;
        }

        public async Task<Dictionary<Status, int>> GetTicketsByStatusAsync()
        {
            return await _context.Tickets
                .GroupBy(t => t.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Status, g => g.Count);
        }

        public async Task<Dictionary<Priority, int>> GetTicketsByPriorityAsync()
        {
            return await _context.Tickets
                .GroupBy(t => t.Priority)
                .Select(g => new { Priority = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Priority, g => g.Count);
        }

        public async Task<Dictionary<string, int>> GetTicketsResolvedPerDepartmentAsync()
        {
            return await _context.Tickets
                .Where(t => t.Status == Status.Resolved || t.Status == Status.Closed)
                .GroupBy(t => t.DepartmentId)
                .Select(g => new { Department = g.Key, Count = g.Count() })
                .ToDictionaryAsync(g => g.Department.ToString(), g => g.Count);
        }

        public async Task<TicketAttachmentDto> AddAttachmentAsync(TicketAttachmentDto attachmentDto)
        {
            var attachment = new TicketAttachment
            {
                FileName = attachmentDto.FileName,
                FilePath = attachmentDto.FilePath,
                ContentType = attachmentDto.ContentType,
                FileSize = attachmentDto.FileSize,
                TicketId = attachmentDto.TicketId
            };

            _context.TicketAttachments.Add(attachment);
            await _context.SaveChangesAsync();

            return attachmentDto;
        }

        public async Task<IEnumerable<TicketAttachmentDto>> GetAttachmentsByTicketIdAsync(int ticketId)
        {
            return await _context.TicketAttachments
                .Where(a => a.TicketId == ticketId)
                .Select(a => new TicketAttachmentDto
                {
                    FileName = a.FileName,
                    FilePath = a.FilePath,
                    ContentType = a.ContentType,
                    FileSize = a.FileSize,
                    TicketId = a.TicketId
                })
                .ToListAsync();
        }
    }
}
