using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.MedicalHistory;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] // 
    public class PatientHistoryController : ControllerBase
    {
        private readonly IPatientHistoryService _historyService;

        public PatientHistoryController(IPatientHistoryService historyService)
        {
            _historyService = historyService;
        }

        [HttpPost("allergy")]
        public async Task<IActionResult> AddAllergy([FromBody] AddAllergyDto dto)
        {
            var result = await _historyService.AddAllergyAsync(dto);
            if (!result) return NotFound(new { message = "Patient is not registered in the system." });

            return Ok(new { message = "Allergy added successfully." });
        }

        [HttpPost("chronic-condition")]
        public async Task<IActionResult> AddChronicCondition([FromBody] AddChronicConditionDto dto) // 
        {
            var result = await _historyService.AddChronicConditionAsync(dto);
            if (!result) return NotFound(new { message = "Patient is not registered in the system." });

            return Ok(new { message = "Chronic condition added successfully." });
        }

        [HttpDelete("allergy/{id}")]
        public async Task<IActionResult> RemoveAllergy(Guid id)
        {
            var result = await _historyService.RemoveAllergyAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }

        [HttpDelete("chronic-condition/{id}")]
        public async Task<IActionResult> RemoveChronicCondition(Guid id)
        {
            var result = await _historyService.RemoveChronicConditionAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}