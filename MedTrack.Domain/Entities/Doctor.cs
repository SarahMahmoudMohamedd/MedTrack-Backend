using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Doctor : BaseEntity<Guid>
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string MedicalLicenseNumber { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public bool IsDeleted { get; set; } = false;

        public ICollection<Visit> Visits { get; set; } = new List<Visit>();
        public ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
    }
}
