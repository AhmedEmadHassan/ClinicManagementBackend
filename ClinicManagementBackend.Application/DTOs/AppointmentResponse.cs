namespace ClinicManagementBackend.Application.DTOs;

public class AppointmentResponse
{
    public Guid Id { get; set; }
    public Guid DoctorId { get; set; }
    public Guid PatientId { get; set; }
    public DateOnly Date { get; set; }
    public int? QueueNumber { get; set; }
    public string Status { get; set; } = string.Empty;
}
