namespace ClinicManagementBackend.Application.DTOs;

public class PatientResponse
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
