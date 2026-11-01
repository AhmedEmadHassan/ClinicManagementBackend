namespace ClinicManagementBackend.Application.DTOs
{
    public class BookAppointmentRequest
    {
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateOnly Date { get; set; }
    }
}
