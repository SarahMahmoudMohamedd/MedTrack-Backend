using MedTrack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class LabInstitution : BaseEntity<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public InstitutionType InstitutionType { get; set; } // 0 = Laboratory, 1 = Radiology, 2 = Both
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        // البيانات الرسمية الموضحة في الـ UI
        public string RegistrationNumber { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string DirectorName { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}
