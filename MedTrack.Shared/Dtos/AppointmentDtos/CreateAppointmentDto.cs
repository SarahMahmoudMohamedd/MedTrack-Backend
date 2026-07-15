using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Shared.Dtos.AppointmentDtos
{
    public class CreateAppointmentDto
    {
        [Required(ErrorMessage = "رقم المريض القومي مطلوب لحجز الموعد.")]
        [RegularExpression(@"^\d{14}$", ErrorMessage = "الرقم القومي يجب أن يكون 14 رقم.")]
        public string PatientNationalId { get; set; } = string.Empty;

        [Required(ErrorMessage = "يجب اختيار الطبيب للموعد.")]
        public Guid DoctorId { get; set; } // 👈 الـ ID بتاع الدكتور اللي أخدناه كوبي من كونتولر الدكاترة

        [Required(ErrorMessage = "تاريخ ووقت الموعد مطلوب.")]
        public DateTime AppointmentDate { get; set; }

        public int DurationInMinutes { get; set; } = 30; // القيمة الافتراضية نص ساعة مثلاً
        public string Type { get; set; } = "Consultation"; // كشف، استشارة، إلخ
        public string Location { get; set; } = "Clinic Room 1";
        public string Notes { get; set; } = string.Empty;
    }
}