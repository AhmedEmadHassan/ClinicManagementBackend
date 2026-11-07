using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;

    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    [HttpPost]
    public async Task<ActionResult<PatientResponse>> Create([FromBody] CreatePatientRequest request)
    {
        try
        {
            var patient = await _patientService.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = patient.Id }, patient);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PatientResponse>>> GetAll()
    {
        var patients = await _patientService.GetAllAsync();
        return Ok(patients);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<PatientResponse>> GetById(Guid id)
    {
        var patient = await _patientService.GetByIdAsync(id);
        if (patient is null)
            return NotFound(new { message = $"Patient with ID '{id}' was not found." });

        return Ok(patient);
    }
}
