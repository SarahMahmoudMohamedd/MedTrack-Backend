using MedTrack.ServicesAbstraction;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class LabAiController : ControllerBase
    {
        private readonly IMedicalAiService _medicalAiService;

        public LabAiController(IMedicalAiService medicalAiService)
        {
            _medicalAiService = medicalAiService;
        }

        // POST /api/LabAi/summarize
        [HttpPost("summarize")]
        public async Task<IActionResult> SummarizeLab(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Please upload a valid lab report PDF file.");

            if (!file.ContentType.Equals("application/pdf", StringComparison.OrdinalIgnoreCase))
                return BadRequest("Only PDF files are allowed.");

            var summary = await _medicalAiService.SummarizeLabPdfAsync(file);
            if (string.IsNullOrEmpty(summary))
                return StatusCode(500, "An error occurred while processing the lab report with the AI service.");

            return Ok(new { summary });
        }
    }
}