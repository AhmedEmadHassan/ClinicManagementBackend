using ClinicManagementBackend.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementBackend.Infrastructure.Data;

public class ClinicDbContext : DbContext
{
    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
    }

    public DbSet<Appointment> Appointments => Set<Appointment>();
    public DbSet<Doctor> Doctors => Set<Doctor>();
    public DbSet<Patient> Patients => Set<Patient>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Doctor configuration
        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasKey(d => d.Id);
            entity.Property(d => d.FullName).IsRequired().HasMaxLength(150);
            entity.Property(d => d.Specialization).IsRequired().HasMaxLength(100);
        });

        // Patient configuration
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.FullName).IsRequired().HasMaxLength(150);
            entity.Property(p => p.PhoneNumber).IsRequired().HasMaxLength(25);
        });

        // Appointment configuration
        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasKey(a => a.Id);

            // Store Enum as readable string in DB ("Scheduled", "CheckedIn", etc.)
            entity.Property(a => a.Status)
                  .HasConversion<string>()
                  .HasMaxLength(20);

            // Index for high-speed queries by doctor and date
            entity.HasIndex(a => new { a.DoctorId, a.Date });
        });
    }
}
