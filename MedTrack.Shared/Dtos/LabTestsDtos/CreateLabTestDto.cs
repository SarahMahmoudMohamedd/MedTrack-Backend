using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.LabTestsDtos
{
    public class CreateLabTestDto
    {
        [Required(ErrorMessage = "Patient National ID is required.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits.")]
        public string PatientNationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The doctor who requested the test must be specified.")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Test name is required.")]
        public string TestName { get; set; } = string.Empty;

        public string? AdditionalNotes { get; set; }
    }
}