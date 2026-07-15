using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction.Security
{
    public interface IAuthService
    {
        Task<PatientCreatedResponseDto?> RegisterPatientAsync(CreatePatientDto dto);

        // تسجيل الدكتور أول مرة
        Task<bool> RegisterDoctorAsync(RegisterDoctorDto dto);

        // تسجيل المعمل أو مركز الأشعة
        Task<bool> RegisterLabAsync(RegisterLabDto dto);

        // تسجيل الدخول الموحد (دكتور / مريض / معمل)
        Task<AuthResponseDto?> LoginAsync(LoginDto dto);

        // تغيير الباسورد الموحد لكل الأدوار
        Task<bool> ChangePasswordAsync(ChangePasswordDto dto);
    }
}
