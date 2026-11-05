using ClinicManagementBackend.Domain.Entities;
using ClinicManagementBackend.Domain.Interfaces;
using ClinicManagementBackend.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace ClinicManagementBackend.Infrastructure.Repositories;

public class DoctorRepository : IDoctorRepository
{
    private readonly ClinicDbContext _context;

    public DoctorRepository(ClinicDbContext context)
    {
        _context = context;
    }

    public async Task<Doctor?> GetByIdAsync(Guid id)
    {
        return await _context.Doctors.FindAsync(id);
    }

    public async Task<IReadOnlyList<Doctor>> GetAllAsync()
    {
        return await _context.Doctors.ToListAsync();
    }

    public async Task AddAsync(Doctor doctor)
    {
        await _context.Doctors.AddAsync(doctor);
        await _context.SaveChangesAsync();
    }
}
