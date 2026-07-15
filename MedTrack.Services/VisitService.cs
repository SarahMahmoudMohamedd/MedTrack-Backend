using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.VisitDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class VisitService : IVisitService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VisitService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<VisitDisplayDto?> CreateVisitAsync(CreateVisitDto dto)
        {
           
            if (string.IsNullOrWhiteSpace(dto.PatientNationalId)) return null;
            var cleanedNationalId = dto.PatientNationalId.Trim();

            var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await patientRepo.FindAsync(p => p.NationalId.Trim() == cleanedNationalId);
            var patient = patients.FirstOrDefault();
            if (patient == null) return null;

            var doctorRepo = _unitOfWork.GetRepository<Doctor, Guid>();
            var doctor = await doctorRepo.GetByIdAsync(dto.DoctorId);
            if (doctor == null || doctor.IsDeleted) return null;

           
            var visit = _mapper.Map<Visit>(dto);

           
            visit.PatientId = patient.Id;
            visit.DoctorId = doctor.Id;
            visit.VisitDate = DateTime.UtcNow;

            
            visit.Medications = new List<PrescribedMedication>();
            if (dto.Medications != null)
            {
                foreach (var medDto in dto.Medications)
                {
                    visit.Medications.Add(new PrescribedMedication
                    {
                        DrugName = medDto.DrugName,
                        Dosage = medDto.Dosage,
                        Frequency = medDto.Frequency
                    });
                }
            }

            
            await _unitOfWork.GetRepository<Visit, Guid>().AddAsync(visit);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<VisitDisplayDto>(visit);
            resultDto.PatientNationalId = patient.NationalId;
            resultDto.PatientFullName = patient.FullName;
            resultDto.DoctorName = $"{doctor.FirstName} {doctor.LastName}".Trim();

            return resultDto;
        }

        public async Task<IEnumerable<VisitDisplayDto>> GetPatientVisitsTimelineAsync(string nationalId)
        {
            var visits = await _unitOfWork.GetRepository<Visit, Guid>()
                .FindAsync(v => v.Patient!.NationalId == nationalId,
                           nameof(Visit.Patient),
                           nameof(Visit.Medications));

            var orderedVisits = visits.OrderByDescending(v => v.VisitDate);

            return _mapper.Map<IEnumerable<VisitDisplayDto>>(orderedVisits);
        }

        public async Task<VisitDisplayDto?> GetVisitByIdAsync(Guid visitId)
        {
            var visit = await _unitOfWork.GetRepository<Visit, Guid>()
                .GetByIdAsync(visitId, nameof(Visit.Patient), nameof(Visit.Medications));

            if (visit == null) return null;

            return _mapper.Map<VisitDisplayDto>(visit);
        }

       
        public async Task<bool> UpdatePrescriptionAsync(Guid visitId, UpdatePrescriptionDto dto)
        {
            var repo = _unitOfWork.GetRepository<Visit, Guid>();
            
            var visit = await repo.GetByIdAsync(visitId, nameof(Visit.Medications));

            if (visit == null) return false;

            
            visit.Diagnosis = dto.Diagnosis;

            
            if (dto.Medications != null)
            {
                visit.Medications.Clear(); 

                foreach (var medDto in dto.Medications)
                {
                   
                    visit.Medications.Add(new PrescribedMedication
                    {
                        DrugName = medDto.MedicationName, 
                        Dosage = medDto.Dosage,
                        Frequency = medDto.Frequency
                    });
                }
            }

            repo.Update(visit);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteVisitAsync(Guid visitId)
        {
            var repo = _unitOfWork.GetRepository<Visit, Guid>();
            var visit = await repo.GetByIdAsync(visitId);
            if (visit == null) return false;

            repo.Delete(visit);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}