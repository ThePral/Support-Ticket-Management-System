using System;
using System.Threading.Tasks;

namespace Core.Interfaces
{
    public interface INotificationService
    {
        Task SendNotificationAsync(int userId, string message);
        Task MarkAsReadAsync(int notificationId);
    }
}