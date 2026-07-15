using MedTrack.Shared.Dtos.DoctorDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface IDoctorService
    {
        Task<DoctorDisplayDto> CreateDoctorAsync(CreateDoctorDto dto);
        Task<IEnumerable<DoctorDisplayDto>> GermanAllDoctorsAsync(); 
        Task<DoctorDisplayDto?> GetDoctorByIdAsync(Guid id);
        Task<bool> DeleteDoctorAsync(Guid id);

    }
}
