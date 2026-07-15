using MedTrack.Shared.Dtos.MedicalHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface IPatientHistoryService
    {
        Task<bool> AddAllergyAsync(AddAllergyDto dto);
        Task<bool> AddChronicConditionAsync(AddChronicConditionDto dto);
        Task<bool> RemoveAllergyAsync(Guid allergyId);
        Task<bool> RemoveChronicConditionAsync(Guid conditionId);
    }
}
