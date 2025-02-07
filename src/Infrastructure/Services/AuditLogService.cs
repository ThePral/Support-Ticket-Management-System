using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Infrastructure.Data;
using Core.Entities;
using Core.Interfaces;

namespace Infrastructure.Services
{
    public class AuditLogService : IAuditLogService
    {
        private readonly AppDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuditLogService> _logger;

        public AuditLogService(AppDbContext context, IHttpContextAccessor httpContextAccessor, ILogger<AuditLogService> logger)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        public async Task LogActivityAsync(string action, string entity, int entityId)
        {
            var user = _httpContextAccessor.HttpContext?.User;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "Anonymous";
            var userRole = user?.FindFirst(ClaimTypes.Role)?.Value ?? "Unknown";
            var ipAddress = _httpContextAccessor.HttpContext?.Connection.RemoteIpAddress?.ToString();

            var log = new AuditLog
            {
                UserId = userId,
                UserRole = userRole,
                Action = action,
                Entity = entity,
                EntityId = entityId,
                IPAddress = ipAddress!
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Audit Log - {action} on {entity} (ID: {entityId}) by User: {userId}");
        }
    }
}