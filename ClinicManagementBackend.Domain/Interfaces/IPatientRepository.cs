using ClinicManagementBackend.Domain.Entities;

namespace ClinicManagementBackend.Domain.Interfaces;

public interface IPatientRepository
{
    Task<Patient?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<Patient>> GetAllAsync();
    Task AddAsync(Patient patient);
}
