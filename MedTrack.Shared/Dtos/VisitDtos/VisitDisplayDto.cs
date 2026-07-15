using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.VisitDtos
{
    public class VisitDisplayDto
    {
        public Guid Id { get; set; }
        public string PatientNationalId { get; set; } = string.Empty;
        public string PatientFullName { get; set; } = string.Empty;
        public DateTime VisitDate { get; set; }
        public string Diagnosis { get; set; } = string.Empty;

        // Vital Signs
        public string BloodPressure { get; set; } = string.Empty;
        public int HeartRate { get; set; }
        public decimal Temperature { get; set; }
        public string DoctorName { get; set; } = string.Empty;

        
        public List<MedicationDisplayDto> Medications { get; set; } = new List<MedicationDisplayDto>();
    }

}
