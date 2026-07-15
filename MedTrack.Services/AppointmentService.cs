using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.Domain.Enums;
using MedTrack.ServicesAbstraction;
using MedTrack.Shared.Dtos.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class AppointmentService : IAppointmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper; 

        public AppointmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<AppointmentDisplayDto?> CreateAppointmentAsync(CreateAppointmentDto dto)
        {
            var patientRepo = _unitOfWork.GetRepository<Patient, Guid>();
            var patients = await patientRepo.FindAsync(p => p.NationalId == dto.PatientNationalId);
            var patient = patients.FirstOrDefault();
            if (patient == null) return null;

            var doctorRepo = _unitOfWork.GetRepository<Doctor, Guid>();
            var doctor = await doctorRepo.GetByIdAsync(dto.DoctorId);
            if (doctor == null) return null;

            var appointment = _mapper.Map<Appointment>(dto);

            appointment.PatientId = patient.Id;
            appointment.DoctorId = doctor.Id;
            appointment.Status = AppointmentStatus.Scheduled; 

            await _unitOfWork.GetRepository<Appointment, Guid>().AddAsync(appointment);
            await _unitOfWork.SaveChangesAsync();

            var resultDto = _mapper.Map<AppointmentDisplayDto>(appointment);
            resultDto.PatientNationalId = patient.NationalId;
            resultDto.PatientFullName = patient.FullName;
            resultDto.DoctorFullName = $"{doctor.FirstName} {doctor.LastName}".Trim();

            return resultDto;
        }

        public async Task<IEnumerable<AppointmentDisplayDto>> GetAppointmentsByDateAsync(DateTime date)
        {
            var appointments = await _unitOfWork.GetRepository<Appointment, Guid>()
                .FindAsync(a => a.AppointmentDate.Date == date.Date,
                           nameof(Appointment.Patient),
                           nameof(Appointment.Doctor));

            return _mapper.Map<IEnumerable<AppointmentDisplayDto>>(appointments);
        }

        public async Task<IEnumerable<AppointmentDisplayDto>> GetPatientAppointmentsAsync(string nationalId)
        {
            var appointments = await _unitOfWork.GetRepository<Appointment, Guid>()
                .FindAsync(a => a.Patient!.NationalId == nationalId,
                           nameof(Appointment.Patient),
                           nameof(Appointment.Doctor));

            return _mapper.Map<IEnumerable<AppointmentDisplayDto>>(appointments);
        }

        public async Task<bool> UpdateStatusAsync(Guid appointmentId, int status)
        {
            var repo = _unitOfWork.GetRepository<Appointment, Guid>();
            var appointment = await repo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;

            appointment.Status = (AppointmentStatus)status;
            repo.Update(appointment);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentDto dto)
        {
            var repo = _unitOfWork.GetRepository<Appointment, Guid>();
            var appointment = await repo.GetByIdAsync(appointmentId);

            if (appointment == null) return false;

            appointment.Status = (MedTrack.Domain.Enums.AppointmentStatus)dto.Status; 
            appointment.Notes = dto.Notes ?? string.Empty;
            appointment.Type = dto.Type; 
            appointment.Location = dto.Location; 
            appointment.DurationInMinutes = dto.DurationInMinutes; 
            appointment.AppointmentDate = dto.AppointmentDate;

            repo.Update(appointment);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAppointmentAsync(Guid appointmentId)
        {
            var repo = _unitOfWork.GetRepository<Appointment, Guid>();
            var appointment = await repo.GetByIdAsync(appointmentId);
            if (appointment == null) return false;

            repo.Delete(appointment);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }


    }
}