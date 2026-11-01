using ClinicManagementBackend.Application.DTOs;

namespace ClinicManagementBackend.Application.Interfaces;

public interface IDoctorService
{
    Task<DoctorResponse> CreateAsync(CreateDoctorRequest request);
    Task<DoctorResponse?> GetByIdAsync(Guid id);
    Task<IReadOnlyList<DoctorResponse>> GetAllAsync();
}
