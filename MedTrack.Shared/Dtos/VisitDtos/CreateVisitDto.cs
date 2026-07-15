using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.VisitDtos
{
    public class CreateVisitDto
    {
        [Required(ErrorMessage = "Patient National ID is required.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "National ID must be exactly 14 digits.")]
        public string PatientNationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "The doctor in charge of the visit must be specified.")]
        public Guid DoctorId { get; set; }

        [Required(ErrorMessage = "Diagnosis is required.")]
        public string Diagnosis { get; set; } = string.Empty;

        public string BloodPressure { get; set; } = string.Empty;
        public int HeartRate { get; set; }
        public decimal Temperature { get; set; }

        public List<PrescribeMedicationDto> Medications { get; set; } = new List<PrescribeMedicationDto>();
    }

    public class PrescribeMedicationDto
    {
        public string DrugName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;
    }
}