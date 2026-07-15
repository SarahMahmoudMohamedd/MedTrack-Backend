using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.PatientDtos
{
    public class CreatePatientDto
    {
        [Required(ErrorMessage = "National ID is required.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits.")]
        public string NationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Full name is required.")]
        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must be between 3 and 200 characters.")]
        public string FullName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Phone number must be exactly 11 digits.")]
        public string PhoneNumber { get; set; } = string.Empty;

        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Date of birth is required.")]
        public DateTime DateOfBirth { get; set; }

        [Range(0, 1, ErrorMessage = "Gender must be 0 (Female) or 1 (Male).")]
        public int Gender { get; set; }

        [Required(ErrorMessage = "Blood type is required.")]
        public string BloodType { get; set; } = string.Empty;

        // Emergency Info
        [Required(ErrorMessage = "Emergency contact name is required.")]
        public string EmergencyContactName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency relationship is required.")]
        public string EmergencyRelationship { get; set; } = string.Empty;

        [Required(ErrorMessage = "Emergency phone number is required.")]
        [RegularExpression(@"^\d{11}$", ErrorMessage = "Emergency phone must be exactly 11 digits.")]
        public string EmergencyPhoneNumber { get; set; } = string.Empty;
    }
}