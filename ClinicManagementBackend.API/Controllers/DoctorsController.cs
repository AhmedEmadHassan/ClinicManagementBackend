using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;

    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    [HttpPost]
    public async Task<ActionResult<DoctorResponse>> Create([FromBody] CreateDoctorRequest request)
    {
        try
        {
            var doctor = await _doctorService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = doctor.Id }, doctor);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DoctorResponse>>> GetAll()
    {
        var doctors = await _doctorService.GetAllAsync();
        return Ok(doctors);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<DoctorResponse>> GetById(Guid id)
    {
        var doctor = await _doctorService.GetByIdAsync(id);
        if (doctor is null)
            return NotFound(new { message = $"Doctor with ID '{id}' was not found." });

        return Ok(doctor);
    }
}
