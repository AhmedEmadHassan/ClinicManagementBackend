namespace ClinicManagementBackend.Application.DTOs;

public class CreateDoctorRequest
{
    public string FullName { get; set; } = string.Empty;
    public string Specialization { get; set; } = string.Empty;
    public int MaxDailyAppointments { get; set; } = 20;
}
