using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AiDtos
{
    public class LabComparisonRequestDto
    {
        public LabTestItemDto CurrentLab { get; set; } = new LabTestItemDto();
        public LabTestItemDto PreviousLab { get; set; } = new LabTestItemDto();
    }
}
