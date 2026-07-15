using AutoMapper;
using MedTrack.Domain.Contracts;
using MedTrack.Domain.Entities;
using MedTrack.ServicesAbstraction;
using MedTrack.ServicesAbstraction.Security;
using MedTrack.Shared.Dtos.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;

        public DoctorService(IUnitOfWork unitOfWork, IMapper mapper, IPasswordService passwordService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _passwordService = passwordService;
        }

        public async Task<DoctorDisplayDto> CreateDoctorAsync(CreateDoctorDto dto)
        {
          
            var doctor = _mapper.Map<Doctor>(dto);

         
            if (!string.IsNullOrEmpty(dto.Password))
            {
                doctor.PasswordHash = _passwordService.HashPassword(dto.Password);
            }

           
            doctor.IsDeleted = false;

            await _unitOfWork.GetRepository<Doctor, Guid>().AddAsync(doctor);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<DoctorDisplayDto>(doctor);
        }

        public async Task<IEnumerable<DoctorDisplayDto>> GermanAllDoctorsAsync()
        {
           
            var allDoctors = await _unitOfWork.GetRepository<Doctor, Guid>().GetAllAsync();
            var activeDoctors = allDoctors.Where(d => !d.IsDeleted);

            return _mapper.Map<IEnumerable<DoctorDisplayDto>>(activeDoctors);
        }

        public async Task<DoctorDisplayDto?> GetDoctorByIdAsync(Guid id)
        {
            var doctor = await _unitOfWork.GetRepository<Doctor, Guid>().GetByIdAsync(id);

           
            if (doctor == null || doctor.IsDeleted) return null;

            return _mapper.Map<DoctorDisplayDto>(doctor);
        }

        public async Task<bool> DeleteDoctorAsync(Guid id)
        {
            var repo = _unitOfWork.GetRepository<Doctor, Guid>();
            var doctor = await repo.GetByIdAsync(id);
            if (doctor == null || doctor.IsDeleted) return false;

            doctor.IsDeleted = true;

      
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
    }
}