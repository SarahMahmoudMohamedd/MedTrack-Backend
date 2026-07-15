using AutoMapper;
using MedTrack.Domain.Entities;
using MedTrack.Shared.Dtos.AppointmentDtos;
using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.DoctorDtos;
using MedTrack.Shared.Dtos.LabTestsDtos;
using MedTrack.Shared.Dtos.MedicalHistory;
using MedTrack.Shared.Dtos.PatientDtos;
using MedTrack.Shared.Dtos.VisitDtos;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MedTrack.Services.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ==========================================
            // 1. AUTH & USER ROLES MAPS (خرائط الحماية والهوية الموحدة)
            // ==========================================

            // دكتور: Register + Login
            CreateMap<RegisterDoctorDto, Doctor>()
                .AfterMap((src, dest) =>
                {
                    var nameParts = src.FullName.Trim().Split(new[] { ' ' }, 2);
                    dest.FirstName = nameParts[0];
                    dest.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                });

            CreateMap<Doctor, AuthResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));

            // معمل: Register + Login 🎯
            CreateMap<RegisterLabDto, LabInstitution>();
            CreateMap<LabInstitution, AuthResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.Name));

            // مريض: Register + Login
            CreateMap<CreatePatientDto, Patient>()
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            CreateMap<Patient, PatientCreatedResponseDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.NationalId))
                .ForMember(dest => dest.TemporaryPassword, opt => opt.Ignore());

            CreateMap<Patient, AuthResponseDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));

            // ==========================================
            // 2. PATIENT PROFILE & HISTORY MAPS
            // ==========================================
            CreateMap<PatientAllergy, PatientItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.AllergyName));

            CreateMap<PatientCondition, PatientItemDto>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.ConditionName));

            CreateMap<Patient, PatientProfileDto>()
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender.ToString()))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => DateTime.Today.Year - src.DateOfBirth.Year));

            CreateMap<AddAllergyDto, PatientAllergy>();
            CreateMap<AddChronicConditionDto, PatientCondition>();

            // ==========================================
            // 3. VISIT & MEDICATION MAPS (النسخة المؤمنة 🎯)
            // ==========================================
            CreateMap<MedicationDisplayDto, PrescribedMedication>().ReverseMap();

            // 👈 التعديل العبقري: منعنا الـ AutoMapper إنه يجتهد في الأدوية والـ IDs عشان ما يضربش 500
            CreateMap<CreateVisitDto, Visit>()
                .ForMember(dest => dest.Medications, opt => opt.Ignore())
                .ForMember(dest => dest.PatientId, opt => opt.Ignore())
                .ForMember(dest => dest.DoctorId, opt => opt.Ignore());

            CreateMap<Visit, VisitDisplayDto>()
                .ForMember(dest => dest.PatientNationalId, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.NationalId : string.Empty))
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.DoctorName, opt => opt.MapFrom(src => src.Doctor != null ? $"{src.Doctor.FirstName} {src.Doctor.LastName}".Trim() : string.Empty))
                .ForMember(dest => dest.Medications, opt => opt.MapFrom(src => src.Medications));

            // ==========================================
            // 4. LAB TEST MAPS
            // ==========================================
            CreateMap<CreateLabTestDto, LabTest>();
            CreateMap<LabTest, LabTestDisplayDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatientNationalId, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.NationalId : string.Empty))
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty));

            // ==========================================
            // 5. DOCTOR MAPS
            // ==========================================
            CreateMap<CreateDoctorDto, Doctor>()
                .AfterMap((src, dest) =>
                {
                    var nameParts = src.FullName.Trim().Split(new[] { ' ' }, 2);
                    dest.FirstName = nameParts[0];
                    dest.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                });

            CreateMap<Doctor, DoctorDisplayDto>()
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}".Trim()));

            // Backward Compatibility mapping
            CreateMap<DoctorRegisterDto, Doctor>()
                .AfterMap((src, dest) =>
                {
                    var nameParts = src.FullName.Trim().Split(new[] { ' ' }, 2);
                    dest.FirstName = nameParts[0];
                    dest.LastName = nameParts.Length > 1 ? nameParts[1] : string.Empty;
                });

            // ==========================================
            // 6. APPOINTMENT MAPS
            // ==========================================
            CreateMap<CreateAppointmentDto, Appointment>();
            CreateMap<Appointment, AppointmentDisplayDto>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.PatientNationalId, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.NationalId : string.Empty))
                .ForMember(dest => dest.PatientFullName, opt => opt.MapFrom(src => src.Patient != null ? src.Patient.FullName : string.Empty))
                .ForMember(dest => dest.DoctorFullName, opt => opt.MapFrom(src => src.Doctor != null ? $"{src.Doctor.FirstName} {src.Doctor.LastName}".Trim() : string.Empty));
        }
    }
}