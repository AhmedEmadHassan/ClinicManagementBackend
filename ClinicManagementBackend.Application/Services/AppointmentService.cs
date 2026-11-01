using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using ClinicManagementBackend.Domain.Entities;
using ClinicManagementBackend.Domain.Enums;
using ClinicManagementBackend.Domain.Interfaces;

namespace ClinicManagementBackend.Application.Services;

public class AppointmentService : IAppointmentService
{
    private readonly IAppointmentRepository _appointmentRepo;
    private readonly IDoctorRepository _doctorRepo;
    private readonly IPatientRepository _patientRepo;

    public AppointmentService(
        IAppointmentRepository appointmentRepo,
        IDoctorRepository doctorRepo,
        IPatientRepository patientRepo)
    {
        _appointmentRepo = appointmentRepo;
        _doctorRepo = doctorRepo;
        _patientRepo = patientRepo;
    }

    public async Task<AppointmentResponse> BookAsync(BookAppointmentRequest request)
    {
        // 1. Input Validation
        if (request.DoctorId == Guid.Empty)
            throw new ArgumentException("Doctor ID is required.");
        if (request.PatientId == Guid.Empty)
            throw new ArgumentException("Patient ID is required.");
        if (request.Date < DateOnly.FromDateTime(DateTime.UtcNow))
            throw new ArgumentException("Cannot book appointments in the past.");

        // 2. Business Rule: Doctor must exist
        var doctor = await _doctorRepo.GetByIdAsync(request.DoctorId);
        if (doctor is null)
            throw new KeyNotFoundException($"Doctor with ID '{request.DoctorId}' was not found.");

        // 3. Business Rule: Patient must exist
        var patient = await _patientRepo.GetByIdAsync(request.PatientId);
        if (patient is null)
            throw new KeyNotFoundException($"Patient with ID '{request.PatientId}' was not found.");

        // 4. Fetch existing appointments for this doctor on the selected date
        var existingAppointments = await _appointmentRepo.GetByDoctorAndDateAsync(request.DoctorId, request.Date);

        // 5. Business Rule: Doctor daily capacity limit
        int activeBookingsCount = existingAppointments.Count(a => a.Status != AppointmentStatus.Cancelled);
        if (activeBookingsCount >= doctor.MaxDailyAppointments)
            throw new InvalidOperationException($"Doctor has reached the daily limit of {doctor.MaxDailyAppointments} appointments for this date.");

        // 6. Business Rule: Prevent duplicate active booking for the same patient with this doctor on this date
        if (existingAppointments.Any(a => a.PatientId == request.PatientId && a.Status != AppointmentStatus.Cancelled))
            throw new InvalidOperationException("Patient already has an active appointment with this doctor on this date.");

        // 7. Create Appointment entity
        var appointment = new Appointment
        {
            Id = Guid.NewGuid(),
            DoctorId = request.DoctorId,
            PatientId = request.PatientId,
            Date = request.Date,
            QueueNumber = null, // Assigned upon physical check-in
            Status = AppointmentStatus.Scheduled
        };

        await _appointmentRepo.AddAsync(appointment);

        return MapToResponse(appointment);
    }

    public async Task<AppointmentResponse> CheckInAsync(Guid appointmentId)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
        if (appointment is null)
            throw new KeyNotFoundException($"Appointment with ID '{appointmentId}' was not found.");

        // Business Rule: Can only check in if currently Scheduled or Skipped (re-queue)
        if (appointment.Status != AppointmentStatus.Scheduled && appointment.Status != AppointmentStatus.Skipped)
            throw new InvalidOperationException("Only scheduled or skipped appointments can be checked in.");

        // Business Rule: Next Queue Number = Highest current number today + 1
        var todayAppointments = await _appointmentRepo.GetByDoctorAndDateAsync(appointment.DoctorId, appointment.Date);
        int nextQueueNumber = (todayAppointments.Max(a => a.QueueNumber) ?? 0) + 1;

        appointment.QueueNumber = nextQueueNumber;
        appointment.Status = AppointmentStatus.CheckedIn;

        await _appointmentRepo.UpdateAsync(appointment);

        return MapToResponse(appointment);
    }

    public async Task<AppointmentResponse> SkipAsync(Guid appointmentId)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
        if (appointment is null)
            throw new KeyNotFoundException($"Appointment with ID '{appointmentId}' was not found.");

        if (appointment.Status != AppointmentStatus.CheckedIn)
            throw new InvalidOperationException("Only currently checked-in appointments can be marked as skipped.");

        appointment.Status = AppointmentStatus.Skipped;

        await _appointmentRepo.UpdateAsync(appointment);

        return MapToResponse(appointment);
    }

    public async Task<AppointmentResponse> CompleteAsync(Guid appointmentId)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
        if (appointment is null)
            throw new KeyNotFoundException($"Appointment with ID '{appointmentId}' was not found.");

        if (appointment.Status != AppointmentStatus.CheckedIn)
            throw new InvalidOperationException("Only currently checked-in appointments can be completed.");

        appointment.Status = AppointmentStatus.Completed;

        await _appointmentRepo.UpdateAsync(appointment);

        return MapToResponse(appointment);
    }

    public async Task<AppointmentResponse> CancelAsync(Guid appointmentId)
    {
        var appointment = await _appointmentRepo.GetByIdAsync(appointmentId);
        if (appointment is null)
            throw new KeyNotFoundException($"Appointment with ID '{appointmentId}' was not found.");

        if (appointment.Status == AppointmentStatus.Completed)
            throw new InvalidOperationException("Completed appointments cannot be cancelled.");

        appointment.Status = AppointmentStatus.Cancelled;

        await _appointmentRepo.UpdateAsync(appointment);

        return MapToResponse(appointment);
    }

    public async Task<IReadOnlyList<AppointmentResponse>> GetDailyQueueAsync(Guid doctorId, DateOnly date)
    {
        var appointments = await _appointmentRepo.GetByDoctorAndDateAsync(doctorId, date);

        return appointments
            .Where(a => a.Status != AppointmentStatus.Cancelled)
            .OrderBy(a => a.QueueNumber.HasValue ? 0 : 1) // Checked-in first
            .ThenBy(a => a.QueueNumber)
            .Select(MapToResponse)
            .ToList();
    }

    private static AppointmentResponse MapToResponse(Appointment a) => new()
    {
        Id = a.Id,
        DoctorId = a.DoctorId,
        PatientId = a.PatientId,
        Date = a.Date,
        QueueNumber = a.QueueNumber,
        Status = a.Status.ToString()
    };
}
