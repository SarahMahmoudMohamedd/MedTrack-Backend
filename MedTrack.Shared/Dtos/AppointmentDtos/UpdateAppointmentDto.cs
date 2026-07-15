using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AppointmentDtos
{
    public class UpdateAppointmentDto
    {
        public int Status { get; set; }
        public string? Notes { get; set; }
        public string Type { get; set; } = string.Empty; 
        public string Location { get; set; } = string.Empty; 
        public int DurationInMinutes { get; set; } 
        public DateTime AppointmentDate { get; set; }
    }
}
