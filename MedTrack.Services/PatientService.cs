using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Enums;
using MedTrack.ServicesAbstraction;
using MedTrack.ServicesAbstraction.Security;
using MedTrack.Shared;
using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class PatientService : IPatientService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public PatientService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<PagedResult<PatientProfileDto>> GetAllPatientsAsync(int page = 1, int pageSize = 10)
        {
            var (patients, totalCount) = await _unitOfWork.GetRepository<Patient, Guid>()
                .GetPagedAsync(
                    p => true,
                    page,
                    pageSize,
                    nameof(Patient.Allergies),
                    nameof(Patient.ChronicConditions));

            return new PagedResult<PatientProfileDto>
            {
                Items = _mapper.Map<List<PatientProfileDto>>(patients),
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<PatientProfileDto?> GetPatientByNationalIdAsync(string nationalId)
        {
            var patients = await _unitOfWork.GetRepository<Patient, Guid>()
                .FindAsync(p => p.NationalId == nationalId,
                           nameof(Patient.ChronicConditions),
                           nameof(Patient.Allergies));

            var patient = patients.FirstOrDefault();
            if (patient == null) return null;

            return _mapper.Map<PatientProfileDto>(patient);
        }

        
        public async Task<PatientCreatedResponseDto?> CreatePatientAsync(CreatePatientDto dto)
        {
            var repo = _unitOfWork.GetRepository<Patient, Guid>();

           
            var existing = await repo.FindAsync(p => p.NationalId == dto.NationalId);
            if (existing.Any()) return null;


            string tempPassword = GenerateTemporaryPassword();

         
            var patient = _mapper.Map<Patient>(dto);
            patient.PasswordHash = _passwordService.HashPassword(tempPassword);

            await repo.AddAsync(patient);
            var success = await _unitOfWork.SaveChangesAsync() > 0;

            if (!success) return null;

            var responseDto = _mapper.Map<PatientCreatedResponseDto>(patient);
            responseDto.TemporaryPassword = tempPassword;
            responseDto.Message = "Patient profile created successfully.";

            return responseDto;
        }

        public async Task<bool> UpdatePatientAsync(string nationalId, CreatePatientDto dto)
        {
            var repo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await repo.FindAsync(p => p.NationalId == nationalId);
            var patient = patients.FirstOrDefault();

            if (patient == null) return false;


            _mapper.Map(dto, patient);

            repo.Update(patient);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeletePatientAsync(string nationalId)
        {
            var repo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await repo.FindAsync(p => p.NationalId == nationalId);
            var patient = patients.FirstOrDefault();

            if (patient == null) return false;

            repo.Delete(patient);
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