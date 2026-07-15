using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.VisitDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class VisitsController : ControllerBase
    {
        private readonly IVisitService _visitService;

        public VisitsController(IVisitService visitService)
        {
            _visitService = visitService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVisitDto dto)
        {
            var result = await _visitService.CreateVisitAsync(dto);
            if (result == null)
                return NotFound(new { message = "Cannot create visit. Patient is not registered in the system." });

            return Ok(result);
        }

       
        [HttpGet("patient/{nationalId}/timeline")] 
        public async Task<IActionResult> GetTimeline(string nationalId)
        {
            var visits = await _visitService.GetPatientVisitsTimelineAsync(nationalId);
            return Ok(visits);
        }

        
        [HttpGet("{id}")] //
        public async Task<IActionResult> GetById(Guid id)
        {
            var visit = await _visitService.GetVisitByIdAsync(id);
            if (visit == null) return NotFound(new { message = "Visit not found." });

            return Ok(visit);
        }

        [HttpPut("{id}/prescription")]
        public async Task<IActionResult> UpdatePrescription(Guid id, [FromBody] UpdatePrescriptionDto dto)
        {
            var result = await _visitService.UpdatePrescriptionAsync(id, dto);
            if (!result)
                return NotFound(new { message = "Cannot update prescription. Visit not found." });

            return Ok(new { message = "Prescription and diagnosis updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _visitService.DeleteVisitAsync(id);
            if (!result) return NotFound(new { message = "Visit not found." });

            return NoContent();
        }
    }
}