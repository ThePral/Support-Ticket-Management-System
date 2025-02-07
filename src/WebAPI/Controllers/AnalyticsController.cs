using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Services;

namespace WebAPI.Controllers
{
    /// <summary>
    /// Analytics-related endpoints for ticket management.
    /// </summary>
    [Route("api/AnalyticsController")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class AnalyticsController : ControllerBase
    {
        private readonly ITicketService _ticketService;

        public AnalyticsController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }

        [HttpGet("average-response-time")]
        public async Task<IActionResult> GetAverageResponseTime()
        {
            var avgResponseTime = await _ticketService.CalculateAverageResponseTimeAsync();
            return Ok(new { AverageResponseTime = avgResponseTime });
        }

        [HttpGet("status-breakdown")]
        public async Task<IActionResult> GetTicketsByStatus()
        {
            var statusBreakdown = await _ticketService.GetTicketsByStatusAsync();
            return Ok(statusBreakdown);
        }

        [HttpGet("priority-breakdown")]
        public async Task<IActionResult> GetTicketsByPriority()
        {
            var priorityBreakdown = await _ticketService.GetTicketsByPriorityAsync();
            return Ok(priorityBreakdown);
        }

        [HttpGet("resolved-per-department")]
        public async Task<IActionResult> GetTicketsResolvedPerDepartment()
        {
            var resolvedPerDepartment = await _ticketService.GetTicketsResolvedPerDepartmentAsync();
            return Ok(resolvedPerDepartment);
        }
    }
}
