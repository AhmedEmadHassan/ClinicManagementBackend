using ClinicManagementBackend.Domain.Entities;

namespace ClinicManagementBackend.Domain.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor?> GetByIdAsync(Guid id);
        Task AddAsync(Doctor doctor);
    }
}
