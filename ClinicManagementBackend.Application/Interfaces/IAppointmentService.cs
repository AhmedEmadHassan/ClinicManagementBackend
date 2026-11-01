using ClinicManagementBackend.Application.DTOs;

namespace ClinicManagementBackend.Application.Interfaces;

public interface IAppointmentService
{
    // 1. Book for a day (validates doctor capacity & duplicates)
    Task<AppointmentResponse> BookAsync(BookAppointmentRequest request);

    // 2. Patient physically arrives, gets next queue number
    Task<AppointmentResponse> CheckInAsync(Guid appointmentId);

    // 3. Doctor calls number, nobody answers
    Task<AppointmentResponse> SkipAsync(Guid appointmentId);

    // 4. Doctor completes consultation
    Task<AppointmentResponse> CompleteAsync(Guid appointmentId);

    // 5. Cancel appointment
    Task<AppointmentResponse> CancelAsync(Guid appointmentId);

    // 6. View the day's queue (sorted by queue number)
    Task<IReadOnlyList<AppointmentResponse>> GetDailyQueueAsync(Guid doctorId, DateOnly date);
}
