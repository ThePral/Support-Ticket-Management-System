using System;

namespace Core.Interfaces
{
    public interface IAuditLogService
    {
        Task LogActivityAsync(string action, string entity, int entityId);
    }
}
