using ClinicManagementBackend.Domain.Enums;

namespace ClinicManagementBackend.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        // The booked clinic day (e.g., 2026-11-05)
        public DateOnly Date { get; set; }
        // Null until the patient arrives at the clinic!
        public int? QueueNumber { get; set; }
        public AppointmentStatus Status { get; set; }
    }
}
