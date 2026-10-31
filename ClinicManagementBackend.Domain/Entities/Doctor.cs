namespace ClinicManagementBackend.Domain.Entities
{
    public class Doctor
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public int MaxDailyAppointments { get; set; } = 20; // default max 20 patients/day
    }
}
