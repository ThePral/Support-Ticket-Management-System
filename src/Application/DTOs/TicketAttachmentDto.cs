using System;

namespace Application.DTOs
{
    public class TicketAttachmentDto
    {
        public string FileName { get; set; } = null!;
        public string FilePath { get; set; } = null!;
        public string ContentType { get; set; } = null!;
        public long FileSize { get; set; }
        public int TicketId { get; set; }
    }
}
