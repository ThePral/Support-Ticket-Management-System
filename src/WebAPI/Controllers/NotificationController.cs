using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Services;
using Core.Interfaces;

namespace WebAPI.Controllers
{
    [Route("api/NotificationController")]
    [ApiController]
    [Authorize]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpPost("add_notification")]
        public async Task<IActionResult> SendNotification([FromBody] string message, [FromQuery] int userId)
        {
            await _notificationService.SendNotificationAsync(userId, message);
            return Ok();
        }

        [HttpPut("{id}/read_notification")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            await _notificationService.MarkAsReadAsync(id);
            return Ok();
        }
    }
}
