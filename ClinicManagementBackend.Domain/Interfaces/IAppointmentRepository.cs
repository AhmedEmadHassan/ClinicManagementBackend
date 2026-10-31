using ClinicManagementBackend.Domain.Entities;

namespace ClinicManagementBackend.Domain.Interfaces
{
    public interface IAppointmentRepository
    {
        Task<Appointment?> GetByIdAsync(Guid id);
        Task<IReadOnlyList<Appointment>> GetByDoctorAndDateAsync(Guid doctorId, DateOnly date);
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
    }
}
