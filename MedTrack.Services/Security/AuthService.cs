using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Enums;
using MedTrack.ServicesAbstraction.Security;
using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordService _passwordService;
        private readonly IJwtTokenService _jwtTokenService;
        private readonly IMapper _mapper;

        public AuthService(
            IUnitOfWork unitOfWork,
            IJwtTokenService jwtTokenService,
            IMapper mapper , IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _jwtTokenService = jwtTokenService;
            _mapper = mapper;
        }

        // 1. تسجيل المريض بالـ Mapping Profile 🦾
        public async Task<PatientCreatedResponseDto?> RegisterPatientAsync(CreatePatientDto dto)
        {
            var repo = _unitOfWork.GetRepository<Patient, Guid>();

            // التأكد من فرادة الرقم القومي
            var existing = await repo.FindAsync(p => p.NationalId == dto.NationalId);
            if (existing.Any()) return null;

            // توليد الباسورد المؤقت العشوائي
            string tempPassword = GenerateTemporaryPassword();

            // استخدام AutoMapper لتحويل الـ DTO إلى Entity أوتوماتيك
            var patient = _mapper.Map<Patient>(dto);

            // إضافة التشفير للباسورد المولد
            patient.PasswordHash = _passwordService.HashPassword(tempPassword);

            await repo.AddAsync(patient);
            var success = await _unitOfWork.SaveChangesAsync() > 0;

            if (!success) return null;

            // تحويل الـ Entity المحفوظة إلى Response DTO بالـ AutoMapper
            var responseDto = _mapper.Map<PatientCreatedResponseDto>(patient);

            // نمرر الباسورد المقروء فقط للـ Response الحالي عشان الدكتور يديه للمريض
            responseDto.TemporaryPassword = tempPassword;
            responseDto.Message = "Patient profile created successfully.";

            return responseDto;
        }

        // 2. تسجيل الدكتور بالـ Mapping
        public async Task<bool> RegisterDoctorAsync(RegisterDoctorDto dto)
        {
            var repo = _unitOfWork.GetRepository<Doctor, Guid>();
            var existing = await repo.FindAsync(d => d.Email == dto.Email);
            if (existing.Any()) return false;

            // Mapping
            var doctor = _mapper.Map<Doctor>(dto);
            doctor.PasswordHash = _passwordService.HashPassword(dto.Password);

            await repo.AddAsync(doctor);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        // 3. تسجيل المعمل بالـ Mapping
        public async Task<bool> RegisterLabAsync(RegisterLabDto dto)
        {
            var repo = _unitOfWork.GetRepository<LabInstitution, Guid>();
            var existing = await repo.FindAsync(l => l.Email == dto.Email);
            if (existing.Any()) return false;

            // Mapping
            var lab = _mapper.Map<LabInstitution>(dto);
            lab.PasswordHash = _passwordService.HashPassword(dto.Password);

            await repo.AddAsync(lab);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<AuthResponseDto?> LoginAsync(LoginDto dto)
        {
            // 1️⃣ أولاً: هنجرب نبحث في جدول الدكاترة بالـ Email 🩺
            var doctorRepo = _unitOfWork.GetRepository<Doctor, Guid>();
            var doctors = await doctorRepo.FindAsync(d => d.Email == dto.Username);
            var doctor = doctors.FirstOrDefault();

            if (doctor != null && _passwordService.VerifyPassword(dto.Password, doctor.PasswordHash))
            {
                var token = _jwtTokenService.GenerateToken(doctor.Id.ToString(), doctor.Email, "Doctor");

                var response = _mapper.Map<AuthResponseDto>(doctor);
                response.Token = token;
                response.Role = "Doctor";
                return response;
            }

            // 2️⃣ ثانياً: لو مطلعش دكتور، هنجرب نبحث في جدول المعامل/مراكز الأشعة بالـ Email 🔬
            var labRepo = _unitOfWork.GetRepository<LabInstitution, Guid>();
            var labs = await labRepo.FindAsync(l => l.Email == dto.Username);
            var lab = labs.FirstOrDefault();

            if (lab != null && _passwordService.VerifyPassword(dto.Password, lab.PasswordHash))
            {
                var token = _jwtTokenService.GenerateToken(lab.Id.ToString(), lab.Email, "Laboratory");

                var response = _mapper.Map<AuthResponseDto>(lab);
                response.Token = token;
                response.Role = "Laboratory";
                return response;
            }

            // 3️⃣ ثالثاً: لو مطلعش معمل، هنجرب نبحث في جدول المرضى بالـ NationalId (الـ Username للمريض) 👤
            var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await patientRepo.FindAsync(p => p.NationalId == dto.Username);
            var patient = patients.FirstOrDefault();

            if (patient != null && _passwordService.VerifyPassword(dto.Password, patient.PasswordHash))
            {
                var token = _jwtTokenService.GenerateToken(patient.Id.ToString(), patient.NationalId, "Patient");

                var response = _mapper.Map<AuthResponseDto>(patient);
                response.Token = token;
                response.Role = "Patient";
                return response;
            }

            // 4️⃣ لو لف على الـ 3 جداول وملقاش الـ Username أو الباسوورد غلط، يرجع null فوراً
            return null;
        }
        public async Task<bool> ChangePasswordAsync(ChangePasswordDto dto)
        {
            // 1️⃣ تغيير الباسوورد للدكتور 🩺
            if (dto.Role == (int)UserRole.Doctor)
            {
                var repo = _unitOfWork.GetRepository<Doctor, Guid>();
                var res = await repo.FindAsync(d => d.Email == dto.Username); // 👈 قرأ من الـ Username الموحد
                var doctor = res.FirstOrDefault();
                if (doctor == null || !_passwordService.VerifyPassword(dto.CurrentPassword, doctor.PasswordHash)) return false;

                doctor.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
                repo.Update(doctor);
            }
            // 2️⃣ تغيير الباسوورد للمريض 👤
            else if (dto.Role == (int)UserRole.Patient)
            {
                var repo = _unitOfWork.GetRepository<Patient, Guid>();
                var res = await repo.FindAsync(p => p.NationalId == dto.Username); // 👈 قرأ من الـ Username الموحد
                var patient = res.FirstOrDefault();
                if (patient == null || !_passwordService.VerifyPassword(dto.CurrentPassword, patient.PasswordHash)) return false;

                patient.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
                repo.Update(patient);
            }
            // 3️⃣ تغيير الباسوورد للمعمل 🔬
            else if (dto.Role == (int)UserRole.Laboratory)
            {
                var repo = _unitOfWork.GetRepository<LabInstitution, Guid>();
                var res = await repo.FindAsync(l => l.Email == dto.Username); // 👈 تعديل: تم تغيير الـ Identifier هنا برضه لـ Username 🎯
                var lab = res.FirstOrDefault();
                if (lab == null || !_passwordService.VerifyPassword(dto.CurrentPassword, lab.PasswordHash)) return false;

                lab.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
                repo.Update(lab);
            }

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        private string GenerateTemporaryPassword()
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            for (int i = 0; i < 8; i++)
            {
                res.Append(validChars[rnd.Next(validChars.Length)]);
            }
            return res.ToString();
        }
    }
}