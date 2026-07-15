using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AppointmentDtos
{
    public class AppointmentDisplayDto
    {
        public Guid Id { get; set; }
        public string PatientNationalId { get; set; } = string.Empty;
        public string PatientFullName { get; set; } = string.Empty;
        public Guid DoctorId { get; set; }
        public string DoctorFullName { get; set; } = string.Empty; // 👈 عرض اسم الدكتور في الـ لستة
        public DateTime AppointmentDate { get; set; }
        public int DurationInMinutes { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }
}
