using MedTrack.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Domain.Entities
{
    public class LabTest : BaseEntity<Guid>
    {
        public Guid PatientId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid? VisitId { get; set; } // ربطه بالزيارة اللي اتطلب منها
        public Guid? LabInstitutionId { get; set; } // المنشأة الطبية اللي نفذت ورفعت النتيجة 💾

        public string TestName { get; set; } = string.Empty; // مثل CBC أو Chest X-Ray
        public TestCategory Category { get; set; } // 0 = Test (تحليل), 1 = Scan (أشعة)

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public LabTestStatus Status { get; set; } // Pending أو Completed

        public string? ResultFilePath { get; set; } // مسار الملف المرفوع (زرار View File في الـ UI)
        public string? AdditionalNotes { get; set; } // نتيجة التحليل المختصرة اللي بتظهر تحت الاسم (مثل All values within normal range)

        public Patient? Patient { get; set; }
        public Doctor? Doctor { get; set; }
        public Visit? Visit { get; set; }
        public LabInstitution? LabInstitution { get; set; }
    }
}
