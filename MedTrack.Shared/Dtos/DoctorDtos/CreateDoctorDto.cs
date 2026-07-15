using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.DoctorDtos
{
    public class CreateDoctorDto
    {
        [Required(ErrorMessage = "Doctor's full name is required.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Medical specialization is required.")]
        public string Specialization { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}