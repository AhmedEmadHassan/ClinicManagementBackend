namespace ClinicManagementBackend.Application.DTOs;

public class DoctorResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int MaxDailyAppointments { get; set; }
}
