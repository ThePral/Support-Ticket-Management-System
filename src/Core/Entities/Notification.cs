using System;

namespace Core.Entities
{
    public class Notification
    {
        public int Id { get; set; }
        public string Message { get; set; } = null!;
        public int UserId { get; set; }
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}