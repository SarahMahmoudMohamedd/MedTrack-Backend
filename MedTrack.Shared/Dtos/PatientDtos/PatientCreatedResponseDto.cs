using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.PatientDtos
{
    public class PatientCreatedResponseDto
    {
        public string Username { get; set; } = string.Empty; // الـ NationalID
        public string TemporaryPassword { get; set; } = string.Empty; 
        public string FullName { get; set; } = string.Empty;
        public string Message { get; set; } = "Patient profile created successfully.";
    }
}
