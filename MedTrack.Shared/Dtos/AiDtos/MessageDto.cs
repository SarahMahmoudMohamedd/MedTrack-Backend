using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AiDtos
{
    public class MessageDto
    {
        public string Message { get; set; } = string.Empty;
        public List<Dictionary<string, string>>? History { get; set; }
    }
}
