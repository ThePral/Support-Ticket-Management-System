using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Services;
using Application.DTOs;
using Core.Enums;
using Core.Interfaces;
using Core.Entities;
using System.Security.Claims;

namespace WebAPI.Controllers
{
    [Route("api/TicketController")]
    [ApiController]
    [Authorize]
    public class TicketController : ControllerBase
    {
        private readonly ITicketService _ticketService;
        private readonly IFileService _fileService;
        private readonly IConfiguration _configuration;
        private readonly ITicketAssignmentService _ticketAssignmentService;
        private readonly ITicketRepository _ticketRepository;
        private readonly IAuditLogService _auditLogService;

        public TicketController(ITicketService ticketService, IFileService fileService,
            IConfiguration configuration, ITicketAssignmentService ticketAssignmentService,
            ITicketRepository ticketRepository, IAuditLogService auditLogService)
        {
            _ticketService = ticketService;
            _fileService = fileService;
            _configuration = configuration;
            _ticketAssignmentService = ticketAssignmentService;
            _ticketRepository = ticketRepository;
            _auditLogService = auditLogService;
        }

        [HttpPost("create")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CreateTicket([FromBody] Ticket ticket)
        {
            await _ticketRepository.AddAsync(ticket);
            await _auditLogService.LogActivityAsync("Created Ticket", "Ticket", ticket.Id);
            return Ok("Ticket created successfully.");
        }

        [HttpGet("assigned")]
        [Authorize(Roles = "SupportAgent")]
        public async Task<IActionResult> GetAssignedTickets()
        {
            if (!int.TryParse(User.FindFirstValue(ClaimTypes.NameIdentifier), out int userId))
            {
                return BadRequest("Invalid user ID.");
            }
            var tickets = await _ticketRepository.GetTicketsByAgentIdAsync(userId);
            return Ok(tickets);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteTicket(int id)
        {
            var success = await _ticketRepository.DeleteAsync(id);
            if (!success) return NotFound("Ticket not found.");

            await _auditLogService.LogActivityAsync("Deleted Ticket", "Ticket", id);

            return Ok("Ticket deleted.");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket([FromBody] CreateTicketDto ticketDto)
        {
            var ticket = await _ticketService.CreateTicketAsync(ticketDto);
            return Ok(ticket);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateTicketStatus(int id, [FromBody] Status status)
        {
            var ticket = await _ticketService.UpdateTicketStatusAsync(id, status);
            return Ok(ticket);
        }

        [HttpGet("department/{departmentId}")]
        public async Task<IActionResult> GetTicketsByDepartment(int departmentId)
        {
            var tickets = await _ticketService.GetTicketsByDepartmentAsync(departmentId);
            return Ok(tickets);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTicketsByUser(int userId)
        {
            var tickets = await _ticketService.GetTicketsByUserAsync(userId);
            return Ok(tickets);
        }

        [HttpPost("{id}/AddAttachments")]
        public async Task<IActionResult> UploadAttachment(int id, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("File cannot be empty");

            var ticket = await _ticketService.GetTicketsByUserAsync(id);
            if (ticket == null) return NotFound("Ticket not found");

            var filePath = await _fileService.UploadFileAsync(file, _configuration["FileStorage:UploadDirectory"]!);
            var attachment = new TicketAttachmentDto
            {
                FileName = file.FileName,
                FilePath = filePath,
                ContentType = file.ContentType,
                FileSize = file.Length,
                TicketId = id
            };

            var savedAttachment = await _ticketService.AddAttachmentAsync(attachment);

            return Ok(savedAttachment);
        }

        [HttpGet("{id}/GetAttachments")]
        public async Task<IActionResult> GetAttachments(int id)
        {
            var attachments = await _ticketService.GetAttachmentsByTicketIdAsync(id);
            return Ok(attachments);
        }

        [HttpPost("{ticketId}/assign")]
        public async Task<IActionResult> AssignTicket(int ticketId)
        {
            var success = await _ticketAssignmentService.AssignTicketAsync(ticketId);
            if (!success)
                return BadRequest("No available agent or ticket already assigned.");

            return Ok("Ticket successfully assigned.");
        }
    }
}
