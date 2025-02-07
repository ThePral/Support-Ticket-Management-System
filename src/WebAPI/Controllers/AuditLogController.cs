using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Core.Enums;

namespace WebAPI.Controllers
{
    [Route("api/AuditLogController")]
    [ApiController]
    [Authorize(Roles = "Administrator")]
    public class AuditLogController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuditLogController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAuditLogs()
        {
            var logs = await _context.AuditLogs.OrderByDescending(a => a.Timestamp).ToListAsync();
            return Ok(logs);
        }
    }
}
