using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.LabTestsDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.IO;
using System.Threading.Tasks;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LabTestsController : ControllerBase
    {
        private readonly ILabTestService _labTestService;
        private readonly IWebHostEnvironment _env; // ✅ Added property for web environment context

        //  Injected IWebHostEnvironment into the constructor
        public LabTestsController(ILabTestService labTestService, IWebHostEnvironment env)
        {
            _labTestService = labTestService;
            _env = env;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRequest([FromBody] CreateLabTestDto dto)
        {
            var result = await _labTestService.CreateLabTestRequestAsync(dto);
            if (result == null) return NotFound(new { message = "Patient is not registered in the system." });

            return Ok(result);
        }

        [HttpPut("{id}/result")]
        [Authorize]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UpdateResult(Guid id, IFormFile file, string? additionalNotes)
        {
            if (file == null || file.Length == 0)
                return BadRequest(new { message = "Please upload a valid file." });

            // Using ContentRootPath to map exactly to the API root directory
            var uploadsFolder = Path.Combine(_env.ContentRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
                Directory.CreateDirectory(uploadsFolder);

            var uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            var filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            var dto = new UpdateLabTestResultDto
            {
                ResultFilePath = $"/uploads/{uniqueFileName}",
                AdditionalNotes = additionalNotes
            };

            var result = await _labTestService.UpdateLabTestResultAsync(id, dto);
            if (!result) return NotFound(new { message = "Lab test not found." });

            return Ok(new { message = "Result file uploaded and saved successfully.", filePath = dto.ResultFilePath });
        }

        [HttpGet("patient/{nationalId}")]
        [Authorize]
        public async Task<IActionResult> GetPatientTests(string nationalId)
        {
            var tests = await _labTestService.GetPatientLabTestsAsync(nationalId);
            return Ok(tests);
        }

        [HttpGet("pending")]
        [Authorize]
        public async Task<IActionResult> GetPendingTests()
        {
            var tests = await _labTestService.GetPendingLabTestsAsync();
            return Ok(tests);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _labTestService.DeleteLabTestAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}