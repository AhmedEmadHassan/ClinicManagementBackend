using ClinicManagementBackend.Domain.Entities;
using ClinicManagementBackend.Domain.Interfaces;
using ClinicManagementBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementBackend.Infrastructure.Repositories;

public class PatientRepository : IPatientRepository
{
    private readonly ClinicDbContext _context;

    public PatientRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<Patient?> GetByIdAsync(Guid id)
    {
        return await _context.Patients.FindAsync(id);
    }

    public async Task<IReadOnlyList<Patient>> GetAllAsync()
    {
        return await _context.Patients.ToListAsync();
    }

    public async Task AddAsync(Patient patient)
    {
        await _context.Patients.AddAsync(patient);
        await _context.SaveChangesAsync();
    }
}
