namespace ClinicManagementBackend.Application.DTOs;

public class CreatePatientRequest
{
    public string FullName { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
}
