using Blog.Models;
using Blog.Services;
using Microsoft.AspNetCore.Mvc;
using Blog.Models;
using Blog.Services;

namespace MyApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ContactController : ControllerBase
{
    private readonly EmailService _emailService;

    public ContactController(EmailService emailService)
    {
        _emailService = emailService;
    }

    [HttpPost]
    public async Task<IActionResult> Send([FromBody] ContactRequest req)
    {
        if (string.IsNullOrWhiteSpace(req.Name) ||
            string.IsNullOrWhiteSpace(req.Email) ||
            string.IsNullOrWhiteSpace(req.Message))
            return BadRequest(new { message = "Tüm alanları doldurun." });

        try
        {
            await _emailService.SendContactEmailAsync(req.Name, req.Email, req.Message);
            return Ok(new { message = "Mesajın başarıyla gönderildi." });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Mail gönderilemedi.", error = ex.Message });
        }
    }
}