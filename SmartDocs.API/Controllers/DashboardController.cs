using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SmartDocs.Domain.Entities;
using SmartDocs.Infrastructure.Data;
using System.Security.Claims;

namespace SmartDocs.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DashboardController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("stats")]
        public IActionResult GetStats()
        {
            return Ok(new
            {
                Documents = _context.Documents.Count(),
                Chats = _context.ChatMessages.Count(),
                Summaries = _context.DocumentSummaries.Count()
            });
        }
    }



}
