using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.AppointmentDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;
        private readonly IPatientService _patientService;

        public AppointmentsController(IAppointmentService appointmentService, IPatientService patientService)
        {
            _appointmentService = appointmentService;
            _patientService = patientService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateAppointmentDto dto)
        {
            var patientExists = await _patientService.GetPatientByNationalIdAsync(dto.PatientNationalId);
            if (patientExists == null)
                return NotFound(new { message = "Sorry, the patient's National ID is not registered in the system." });

            var result = await _appointmentService.CreateAppointmentAsync(dto);
            if (result == null)
                return NotFound(new { message = "Sorry, the provided Doctor ID is invalid or not registered." });

            return Ok(result);
        }

        [HttpGet("date/{date}")]
        public async Task<IActionResult> GetByDate(DateTime date)
        {
            var appointments = await _appointmentService.GetAppointmentsByDateAsync(date);
            return Ok(appointments);
        }

       
        [HttpGet("patient/{nationalId}")]
        public async Task<IActionResult> GetByPatient(string nationalId)
        {
            var appointments = await _appointmentService.GetPatientAppointmentsAsync(nationalId);
            return Ok(appointments);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAppointment(Guid id, [FromBody] UpdateAppointmentDto dto)
        {
            var result = await _appointmentService.UpdateAppointmentAsync(id, dto);
            if (!result) return NotFound(new { message = "Appointment not found." });

            return Ok(new { message = "Appointment updated successfully with all details." });
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(Guid id, [FromQuery] int status)
        {
            var result = await _appointmentService.UpdateStatusAsync(id, status);
            if (!result) return NotFound("Appointment not found.");
            return Ok(new { message = "Appointment status updated successfully." });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _appointmentService.DeleteAppointmentAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}