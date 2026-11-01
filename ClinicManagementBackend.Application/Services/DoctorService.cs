using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using ClinicManagementBackend.Domain.Entities;
using ClinicManagementBackend.Domain.Interfaces;

namespace ClinicManagementBackend.Application.Services;

public class DoctorService : IDoctorService
{
    private readonly IDoctorRepository _doctorRepo;

    public DoctorService(IDoctorRepository doctorRepo)
    {
        _doctorRepo = doctorRepo;
    }

    public async Task<DoctorResponse> CreateAsync(CreateDoctorRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FullName))
            throw new ArgumentException("Doctor name is required.");
        if (string.IsNullOrWhiteSpace(request.Specialization))
            throw new ArgumentException("Doctor specialization is required.");
        if (request.MaxDailyAppointments <= 0)
            throw new ArgumentException("Max daily appointments must be greater than zero.");

        var doctor = new Doctor
        {
            Id = Guid.NewGuid(),
            FullName = request.FullName.Trim(),
            Specialization = request.Specialization.Trim(),
            MaxDailyAppointments = request.MaxDailyAppointments
        };

        await _doctorRepo.AddAsync(doctor);

        return new DoctorResponse
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            Specialization = doctor.Specialization,
            MaxDailyAppointments = doctor.MaxDailyAppointments
        };
    }

    public async Task<DoctorResponse?> GetByIdAsync(Guid id)
    {
        var doctor = await _doctorRepo.GetByIdAsync(id);
        if (doctor is null) return null;

        return new DoctorResponse
        {
            Id = doctor.Id,
            FullName = doctor.FullName,
            Specialization = doctor.Specialization,
            MaxDailyAppointments = doctor.MaxDailyAppointments
        };
    }

    public async Task<IReadOnlyList<DoctorResponse>> GetAllAsync()
    {
        var doctors = await _doctorRepo.GetAllAsync();
        return doctors.Select(d => new DoctorResponse
        {
            Id = d.Id,
            FullName = d.FullName,
            Specialization = d.Specialization,
            MaxDailyAppointments = d.MaxDailyAppointments
        }).ToList();
    }
}
