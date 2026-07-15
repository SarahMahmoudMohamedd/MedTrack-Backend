using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.VisitDtos
{
    public class UpdatePrescriptionDto
    {
        [Required(ErrorMessage = "Diagnosis is required to update the prescription.")]
        public string Diagnosis { get; set; } = string.Empty;

        public string? DoctorNotes { get; set; }

       
        public List<VisitMedicationDto> Medications { get; set; } = new List<VisitMedicationDto>();
    }
    public class VisitMedicationDto
    {
        [Required(ErrorMessage = "Medication name is required.")]
        public string MedicationName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Dosage is required (e.g., 500mg).")]
        public string Dosage { get; set; } = string.Empty;

        [Required(ErrorMessage = "Frequency is required (e.g., 3 times a day).")]
        public string Frequency { get; set; } = string.Empty;

        [Required(ErrorMessage = "Duration is required (e.g., 7 days).")]
        public string Duration { get; set; } = string.Empty;

        public string? SpecialInstructions { get; set; } 
    }
}
