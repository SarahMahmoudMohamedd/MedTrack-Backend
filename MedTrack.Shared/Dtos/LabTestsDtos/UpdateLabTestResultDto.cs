using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.LabTestsDtos
{
    public class UpdateLabTestResultDto
    {
        [Required(ErrorMessage = "مسار ملف النتيجة مطلوب.")]
        public string ResultFilePath { get; set; } = string.Empty; 
        public string? AdditionalNotes { get; set; } 
    }
}
