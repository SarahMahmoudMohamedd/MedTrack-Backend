using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.PatientDtos
{
    public class PatientItemDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
