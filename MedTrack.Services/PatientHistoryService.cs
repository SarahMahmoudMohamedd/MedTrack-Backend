using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.MedicalHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class PatientHistoryService : IPatientHistoryService
    {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IMapper _mapper; 

            public PatientHistoryService(IUnitOfWork unitOfWork, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _mapper = mapper;
            }

            public async Task<bool> AddAllergyAsync(AddAllergyDto dto)
            {
                var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
                var patients = await patientRepo.FindAsync(p => p.NationalId == dto.PatientNationalId);
                var patient = patients.FirstOrDefault();

                if (patient == null) return false;

                var allergy = _mapper.Map<PatientAllergy>(dto);
                allergy.PatientId = patient.Id; 

                await _unitOfWork.GetRepository<PatientAllergy, Guid>().AddAsync(allergy);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            public async Task<bool> AddChronicConditionAsync(AddChronicConditionDto dto)
            {
                var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
                var patients = await patientRepo.FindAsync(p => p.NationalId == dto.PatientNationalId);
                var patient = patients.FirstOrDefault();

                if (patient == null) return false;

                var condition = _mapper.Map<PatientCondition>(dto);
                condition.PatientId = patient.Id; 

                await _unitOfWork.GetRepository<PatientCondition, Guid>().AddAsync(condition);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            public async Task<bool> RemoveAllergyAsync(Guid allergyId)
            {
                var repo = _unitOfWork.GetRepository<PatientAllergy, Guid>();
                var allergy = await repo.GetByIdAsync(allergyId);
                if (allergy == null) return false;

                repo.Delete(allergy);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }

            public async Task<bool> RemoveChronicConditionAsync(Guid conditionId)
            {
                var repo = _unitOfWork.GetRepository<PatientCondition, Guid>();
                var condition = await repo.GetByIdAsync(conditionId);
                if (condition == null) return false;

                repo.Delete(condition);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
        }
    }