using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.AiDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class ChatController : ControllerBase
    {
        private readonly IMedicalAiService _medicalAiService;
        private readonly IUnitOfWork _unitOfWork;

        public ChatController(IMedicalAiService medicalAiService, IUnitOfWork unitOfWork)
        {
            _medicalAiService = medicalAiService;
            _unitOfWork = unitOfWork;
        }

        // POST /api/Chat/ask/{nationalId}
        [HttpPost("ask/{nationalId}")]
        public async Task<IActionResult> AskBot(string nationalId, [FromBody] MessageDto messageDto)
        {
            var result = await _medicalAiService.AskChatbotWithContextAsync(nationalId, messageDto);

            if (result == null)
                return StatusCode(500, "The medical AI service did not respond. Please try again later.");

            return Ok(result);
        }

        // POST /api/Chat/upload-prescription
        [HttpPost("upload-prescription")]
        public async Task<IActionResult> UploadPrescription(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Please provide a valid prescription image.");

            var allowedTypes = new[] { "image/jpeg", "image/png", "image/jpg" };
            if (!allowedTypes.Contains(image.ContentType.ToLower()))
                return BadRequest("Only JPG and PNG images are allowed.");

            var ocrResult = await _medicalAiService.ProcessPrescriptionOcrAsync(image);
            if (ocrResult == null)
                return StatusCode(500, "OCR service failed to extract medications from the prescription image.");

            return Ok(ocrResult);
        }

        // POST /api/Chat/labs/comparison-graph
        [HttpPost("labs/comparison-graph")]
        public async Task<IActionResult> GetComparisonGraph([FromBody] LabComparisonRequestDto requestDto)
        {
            var result = await _medicalAiService.GetComparisonGraphDataAsync(requestDto);

            if (result == null)
                return BadRequest("Invalid lab data provided.");

            return Ok(result);
        }
    }
}