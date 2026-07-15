using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AuthDtos
{
    public class RegisterLabDto
    {
        public string Name { get; set; } = string.Empty;
        public int Type { get; set; } // 0 = Lab, 1 = Radiology, 2 = Both
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string RegistrationNumber { get; set; } = string.Empty;
        public string LicenseNumber { get; set; } = string.Empty;
        public string DirectorName { get; set; } = string.Empty;
        public DateTime EstablishedDate { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Website { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
