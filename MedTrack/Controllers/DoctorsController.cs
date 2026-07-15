using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.DoctorDtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MedTrack.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly IDoctorService _doctorService;

        public DoctorsController(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }

       
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromBody] CreateDoctorDto dto)
        {
            var result = await _doctorService.CreateDoctorAsync(dto);
            return Ok(result);
        }

       
        [HttpGet]
        [Authorize] 
        public async Task<IActionResult> GetAll()
        {
            var doctors = await _doctorService.GermanAllDoctorsAsync();
            return Ok(doctors);
        }

        [HttpGet("{id}")]
        [Authorize] 
        public async Task<IActionResult> GetById(Guid id)
        {
            var doctor = await _doctorService.GetDoctorByIdAsync(id);
            if (doctor == null) return NotFound(new { message = "Doctor not found." });

            return Ok(doctor);
        }

        
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _doctorService.DeleteDoctorAsync(id);
            if (!result) return NotFound();
            return NoContent();
        }
    }
}