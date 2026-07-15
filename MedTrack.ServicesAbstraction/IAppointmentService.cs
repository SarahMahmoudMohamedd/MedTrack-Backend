using MedTrack.Shared.Dtos.AppointmentDtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedTrack.ServicesAbstraction
{
    public interface IAppointmentService
    {
        Task<AppointmentDisplayDto?> CreateAppointmentAsync(CreateAppointmentDto dto);
        Task<IEnumerable<AppointmentDisplayDto>> GetAppointmentsByDateAsync(DateTime date);
        Task<IEnumerable<AppointmentDisplayDto>> GetPatientAppointmentsAsync(string nationalId);
        Task<bool> UpdateStatusAsync(Guid appointmentId, int status);
        Task<bool> UpdateAppointmentAsync(Guid appointmentId, UpdateAppointmentDto dto);
        Task<bool> DeleteAppointmentAsync(Guid appointmentId);
    }
}
