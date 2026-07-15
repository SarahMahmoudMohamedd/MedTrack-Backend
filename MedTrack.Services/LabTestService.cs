using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Enums;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.LabTestsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class LabTestService : ILabTestService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public LabTestService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<LabTestDisplayDto?> CreateLabTestRequestAsync(CreateLabTestDto dto)
        {
            var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await patientRepo.FindAsync(p => p.NationalId == dto.PatientNationalId);
            var patient = patients.FirstOrDefault();
            if (patient == null) return null;

            var doctorRepo = _unitOfWork.GetRepository<Doctor, Guid>();
            var doctor = await doctorRepo.GetByIdAsync(dto.DoctorId);
            if (doctor == null) return null;

            var labTest = _mapper.Map<LabTest>(dto);

            labTest.PatientId = patient.Id;
            labTest.DoctorId = doctor.Id;
            labTest.OrderDate = DateTime.UtcNow;
            labTest.Status = LabTestStatus.Pending; 
            labTest.ResultFilePath = null;

            await _unitOfWork.GetRepository<LabTest, Guid>().AddAsync(labTest);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<LabTestDisplayDto>(labTest);
            resultDto.PatientNationalId = patient.NationalId;
            resultDto.PatientFullName = patient.FullName;

            return resultDto;
        }

        public async Task<bool> UpdateLabTestResultAsync(Guid testId, UpdateLabTestResultDto dto)
        {
            var repo = _unitOfWork.GetRepository<LabTest, Guid>();
            var labTest = await repo.GetByIdAsync(testId);

            if (labTest == null) return false;

            labTest.ResultFilePath = dto.ResultFilePath;
            labTest.AdditionalNotes = dto.AdditionalNotes;
            labTest.Status = LabTestStatus.Completed; 

            repo.Update(labTest);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<IEnumerable<LabTestDisplayDto>> GetPatientLabTestsAsync(string nationalId)
        {
            var tests = await _unitOfWork.GetRepository<LabTest, Guid>()
                .FindAsync(t => t.Patient!.NationalId == nationalId, nameof(LabTest.Patient));

            var orderedTests = tests.OrderByDescending(t => t.OrderDate);

            return _mapper.Map<IEnumerable<LabTestDisplayDto>>(orderedTests);
        }

        public async Task<IEnumerable<LabTestDisplayDto>> GetPendingLabTestsAsync()
        {
            var tests = await _unitOfWork.GetRepository<LabTest, Guid>()
                .FindAsync(t => t.Status == LabTestStatus.Pending, nameof(LabTest.Patient));

            return _mapper.Map<IEnumerable<LabTestDisplayDto>>(tests);
        }

        public async Task<bool> DeleteLabTestAsync(Guid testId)
        {
            var repo = _unitOfWork.GetRepository<LabTest, Guid>();
            var labTest = await repo.GetByIdAsync(testId);
            if (labTest == null) return false;

            repo.Delete(labTest);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}