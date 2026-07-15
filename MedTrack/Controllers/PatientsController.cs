using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.PatientDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class PatientsController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientsController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _patientService.GetAllPatientsAsync(page, pageSize);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientDto dto)
        {
            var createdPatientDto = await _patientService.CreatePatientAsync(dto);

            if (createdPatientDto == null)
                return BadRequest("Patient already exists.");

            return CreatedAtAction(
                nameof(GetByNationalId),
                new { nationalId = dto.NationalId },
                createdPatientDto);
        }

        [HttpGet("{nationalId}")] 
        public async Task<IActionResult> GetByNationalId(string nationalId)
        {
            var patient = await _patientService.GetPatientByNationalIdAsync(nationalId);
            if (patient == null)
                return NotFound(new { message = "Patient is not registered in the system." });

            return Ok(patient);
        }

        [HttpPut("{nationalId}")]
        public async Task<IActionResult> Update(string nationalId, [FromBody] CreatePatientDto dto)
        {
            var result = await _patientService.UpdatePatientAsync(nationalId, dto);
            if (!result) return NotFound("Patient not found.");

            return Ok(new { message = "Patient updated successfully" });
        }

        [HttpDelete("{nationalId}")]
        public async Task<IActionResult> Delete(string nationalId)
        {
            var result = await _patientService.DeletePatientAsync(nationalId);
            if (!result) return NotFound();

            return NoContent();
        }
    }
}