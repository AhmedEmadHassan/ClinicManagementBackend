using ClinicManagementBackend.Domain.Entities;

namespace ClinicManagementBackend.Domain.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient?> GetByIdAsync(Guid id);
        Task AddAsync(Patient patient);
    }
}
