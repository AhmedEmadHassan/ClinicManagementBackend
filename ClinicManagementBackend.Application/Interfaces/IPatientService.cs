using ClinicManagementBackend.Application.DTOs;

namespace ClinicManagementBackend.Application.Interfaces;

public interface IPatientService
{
    Task<PatientResponse> CreateAsync(CreatePatientRequest request);
    Task<PatientResponse?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<PatientResponse>> GetAllAsync();
}
