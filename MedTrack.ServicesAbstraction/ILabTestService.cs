using MedTrack.Shared.Dtos.LabTestsDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface ILabTestService
    {
        Task<LabTestDisplayDto?> CreateLabTestRequestAsync(CreateLabTestDto dto);
        Task<bool> UpdateLabTestResultAsync(Guid testId, UpdateLabTestResultDto dto);
        Task<IEnumerable<LabTestDisplayDto>> GetPatientLabTestsAsync(string nationalId);
        Task<IEnumerable<LabTestDisplayDto>> GetPendingLabTestsAsync(); 
        Task<bool> DeleteLabTestAsync(Guid testId);
    }
}
