using MedTrack.ServicesAbstraction;
using MedTrack.ServicesAbstraction.Security;
using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.PatientDtos;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace MedTrack.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register-doctor")]
        public async Task<IActionResult> RegisterDoctor([FromBody] RegisterDoctorDto dto)
        {
            var result = await _authService.RegisterDoctorAsync(dto);
            if (!result) return BadRequest("Email already exists or registration failed.");
            return Ok("Doctor registered successfully.");
        }

        [HttpPost("register-lab")]
        public async Task<IActionResult> RegisterLab([FromBody] RegisterLabDto dto)
        {
            var result = await _authService.RegisterLabAsync(dto);
            if (!result) return BadRequest("Email already exists or registration failed.");
            return Ok("Laboratory/Radiology institution registered successfully.");
        }

        [HttpPost("register-patient")]
        public async Task<IActionResult> RegisterPatient([FromBody] CreatePatientDto dto)
        {
            var result = await _authService.RegisterPatientAsync(dto);
            if (result == null) return BadRequest("National ID already exists or registration failed.");
            return Ok(result); 
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var response = await _authService.LoginAsync(dto); 
            if (response == null) return Unauthorized("Invalid credentials or role.");
            return Ok(response);
        }

        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto dto)
        {
            var result = await _authService.ChangePasswordAsync(dto);
            if (!result) return BadRequest("Invalid current password or identity verification failed.");
            return Ok("Password updated successfully.");
        }
    }
}