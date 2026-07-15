using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.MedicalHistory
{
    public class AddAllergyDto
    {
        [Required(ErrorMessage = "Patient National ID is required.")]
        public string PatientNationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "Allergy name is required.")]
        public string AllergyName { get; set; } = string.Empty;
    }
}