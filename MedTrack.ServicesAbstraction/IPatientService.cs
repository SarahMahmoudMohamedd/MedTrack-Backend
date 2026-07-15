using MedTrack.Shared;
using MedTrack.Shared.Dtos.AuthDtos;
using MedTrack.Shared.Dtos.PatientDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface IPatientService
    {
        Task<PagedResult<PatientProfileDto>> GetAllPatientsAsync(int page = 1, int pageSize = 10);
        Task<PatientProfileDto?> GetPatientByNationalIdAsync(string nationalId);

        // التعديل السحري هنا بالـ DTO الجديد 🎯
        Task<PatientCreatedResponseDto?> CreatePatientAsync(CreatePatientDto dto);

        Task<bool> UpdatePatientAsync(string nationalId, CreatePatientDto dto);
        Task<bool> DeletePatientAsync(string nationalId);
    }
}
