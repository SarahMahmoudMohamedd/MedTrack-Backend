using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.MedicalHistory
{
    public class AddChronicConditionDto
    {
        [Required(ErrorMessage = "Patient National ID is required.")]
        public string PatientNationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Chronic condition name is required.")]
        public string ConditionName { get; set; } = string.Empty;
    }
}