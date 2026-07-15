using MedTrack.Shared.Dtos.VisitDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface IVisitService
    {
        Task<VisitDisplayDto?> CreateVisitAsync(CreateVisitDto dto);
        Task<IEnumerable<VisitDisplayDto>> GetPatientVisitsTimelineAsync(string nationalId);
        Task<VisitDisplayDto?> GetVisitByIdAsync(Guid visitId);
        Task<bool> UpdatePrescriptionAsync(Guid visitId, UpdatePrescriptionDto dto);
        Task<bool> DeleteVisitAsync(Guid visitId);
    }
}
