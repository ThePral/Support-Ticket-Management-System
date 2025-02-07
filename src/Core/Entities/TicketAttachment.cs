using System;

namespace Core.Entities
{
    public class TicketAttachment
    {
        public int Id { get; set; }
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
        public int TicketId { get; set; }
        public Ticket Ticket { get; set; } = null!;
    }
}