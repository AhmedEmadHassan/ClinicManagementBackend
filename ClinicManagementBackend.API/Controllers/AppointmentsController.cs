using ClinicManagementBackend.Application.DTOs;
using ClinicManagementBackend.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ClinicManagementBackend.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AppointmentsController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;

    public AppointmentsController(IAppointmentService appointmentService)
    {
        _appointmentService = appointmentService;
    }

    // 1. Book an appointment for a specific date
    [HttpPost]
    public async Task<ActionResult<AppointmentResponse>> Book([FromBody] BookAppointmentRequest request)
    {
        try
        {
            var appointment = await _appointmentService.BookAsync(request);
            return StatusCode(StatusCodes.Status201Created, appointment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new { message = ex.Message });
        }
    }

    // 2. Physical Check-in when patient arrives at clinic (assigns QueueNumber)
    [HttpPost("{id:guid}/check-in")]
    public async Task<ActionResult<AppointmentResponse>> CheckIn(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.CheckInAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 3. Skip a checked-in patient if called and absent
    [HttpPost("{id:guid}/skip")]
    public async Task<ActionResult<AppointmentResponse>> Skip(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.SkipAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 4. Complete consultation
    [HttpPost("{id:guid}/complete")]
    public async Task<ActionResult<AppointmentResponse>> Complete(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.CompleteAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 5. Cancel appointment
    [HttpPost("{id:guid}/cancel")]
    public async Task<ActionResult<AppointmentResponse>> Cancel(Guid id)
    {
        try
        {
            var appointment = await _appointmentService.CancelAsync(id);
            return Ok(appointment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    // 6. View the daily queue for a doctor (sorted by queue number)
    [HttpGet("queue")]
    public async Task<ActionResult<IReadOnlyList<AppointmentResponse>>> GetDailyQueue(
        [FromQuery] Guid doctorId,
        [FromQuery] DateOnly date)
    {
        if (doctorId == Guid.Empty)
            return BadRequest(new { message = "DoctorId query parameter is required." });

        var queue = await _appointmentService.GetDailyQueueAsync(doctorId, date);
        return Ok(queue);
    }
}
