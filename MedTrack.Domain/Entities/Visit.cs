using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Visit : BaseEntity<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime VisitDate { get; set; } = DateTime.UtcNow;
        public string Diagnosis { get; set; } = string.Empty;

        // Vital Signs جعلناها Nullable (?) عشان الدكتور يملا اللي هو عايزه بس 👍
        public string? BloodPressure { get; set; }
        public int? HeartRate { get; set; }
        public decimal? Temperature { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public ICollection<PrescribedMedication> Medications { get; set; } = new List<PrescribedMedication>();
    }

    public class PrescribedMedication : BaseEntity<Guid>
    {
        public Guid VisitId { get; set; }
        public string DrugName { get; set; } = string.Empty;
        public string Dosage { get; set; } = string.Empty;
        public string Frequency { get; set; } = string.Empty;

        public Visit? Visit { get; set; }
    }
}