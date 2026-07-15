using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class Notification : BaseEntity<Guid>
    {
        public Guid UserId { get; set; }
        public string Text { get; set; } = string.Empty;
        public bool IsRead { get; set; }
    }
}
