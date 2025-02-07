using System;
using Microsoft.AspNetCore.SignalR;

namespace Infrastructure.Notifications
{
    public class TicketHub : Hub
{
    public async Task SendTicketNotification(string agentId, string message)
    {
        await Clients.User(agentId).SendAsync("ReceiveTicketNotification", message);
    }
}
}