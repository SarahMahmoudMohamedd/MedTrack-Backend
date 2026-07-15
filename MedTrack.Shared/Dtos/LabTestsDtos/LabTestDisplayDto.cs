using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.LabTestsDtos
{
    public class LabTestDisplayDto
    {
        public Guid Id { get; set; }
        public string PatientNationalId { get; set; } = string.Empty;
        public string PatientFullName { get; set; } = string.Empty;
        public string TestName { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = string.Empty; 
        public string? ResultFilePath { get; set; }
        public string? AdditionalNotes { get; set; }
    }
}
