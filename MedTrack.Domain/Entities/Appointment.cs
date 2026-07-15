using MedTrack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Appointment : BaseEntity<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime AppointmentDate { get; set; }
        public int DurationInMinutes { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public AppointmentStatus Status { get; set; }

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
    }
}