using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AiDtos
{
    public class LabTestItemDto
    {
        public string TestName { get; set; } = string.Empty;
        public string OrderDate { get; set; } = string.Empty;
        public string AdditionalNotes { get; set; } = string.Empty;
    }
}
