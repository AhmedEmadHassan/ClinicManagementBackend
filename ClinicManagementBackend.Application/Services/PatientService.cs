using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using ClinicManagementBackend.Domain.Entities;
using ClinicManagementBackend.Domain.Interfaces;

namespace ClinicManagementBackend.Application.Services;

public class PatientService : IPatientService
{
    private readonly IPatientRepository _patientRepo;

    public PatientService(IPatientRepository patientRepo)
    {
        _patientRepo = patientRepo;
    }

    public async Task<PatientResponse> CreateAsync(CreatePatientRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Patient name is required.");
        if (string.IsNullOrWhiteSpace(request.PhoneNumber))
            throw new ArgumentException("Phone number is required.");

        var patient = new Patient
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            PhoneNumber = request.PhoneNumber.Trim()
        };

        await _patientRepo.AddAsync(patient);

        return new PatientResponse
        {
            Id = patient.Id,
            FullName = patient.FullName,
            PhoneNumber = patient.PhoneNumber
        };
    }

    public async Task<PatientResponse?> GetByIdAsync(Guid id)
    {
        var patient = await _patientRepo.GetByIdAsync(id);
        if (patient is null) return null;

        return new PatientResponse
        {
            Id = patient.Id,
            FullName = patient.FullName,
            PhoneNumber = patient.PhoneNumber
        };
    }

    public async Task<IReadOnlyList<PatientResponse>> GetAllAsync()
    {
        var patients = await _patientRepo.GetAllAsync();
        return patients.Select(p => new PatientResponse
        {
            Id = p.Id,
            FullName = p.FullName,
            PhoneNumber = p.PhoneNumber
        }).ToList();
    }
}
