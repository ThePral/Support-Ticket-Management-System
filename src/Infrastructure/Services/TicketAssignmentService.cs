using System;
using Core.Interfaces;
using Microsoft.AspNetCore.SignalR;
using Infrastructure.Notifications;

namespace Infrastructure.Services
{
    public class TicketAssignmentService : ITicketAssignmentService
    {
        private readonly ITicketRepository _ticketRepository;
        private readonly IAgentRepository _agentRepository;
        private readonly IHubContext<TicketHub> _hubContext;

        public TicketAssignmentService(ITicketRepository ticketRepository, IAgentRepository agentRepository, IHubContext<TicketHub> hubContext)
        {
            _ticketRepository = ticketRepository;
            _agentRepository = agentRepository;
            _hubContext = hubContext;
        }

        public async Task<bool> AssignTicketAsync(int ticketId)
        {
            var ticket = await _ticketRepository.GetByIdAsync(ticketId);
            if (ticket == null || ticket.AgentId != null)
                return false;

            var agent = await _agentRepository.GetAvailableAgentAsync(ticket.DepartmentType);
            if (agent == null)
                return false;

            ticket.AgentId = agent.Id;
            agent.AssignedTicketsCount++;

            await _ticketRepository.UpdateAsync(ticket);
            await _agentRepository.UpdateAsync(agent);

            var message = $"New Ticket Assigned: {ticket.Title} (Priority: {ticket.Priority})";
            await _hubContext.Clients.User(agent.Id.ToString()).SendAsync("ReceiveTicketNotification", message);

            return true;
        }
    }
}