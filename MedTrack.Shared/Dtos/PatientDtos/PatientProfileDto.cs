using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.PatientDtos
{
    public class PatientProfileDto
    {
        public Guid Id { get; set; }
        public string NationalId { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public string BloodType { get; set; } = string.Empty;

        
        public List<PatientItemDto> ChronicConditions { get; set; } = new();
        public List<PatientItemDto> Allergies { get; set; } = new();
    }
}