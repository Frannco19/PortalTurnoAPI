using Application.DTOs.Appointments;
using Application.Interfaces.Repositories;
using Application.Services;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TurnosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly AppointmentService _appointmentService;

        public AppointmentsController(
            IAppointmentRepository appointmentRepository,
            AppointmentService appointmentService)
        {
            _appointmentRepository = appointmentRepository;
            _appointmentService = appointmentService;
        }

        // GET: /api/appointments?date=2025-01-01&professionalId=1
        [HttpGet]
        public async Task<IActionResult> GetByDate([FromQuery] DateTime date, [FromQuery] int? professionalId)
        {
            var appointments = await _appointmentRepository.GetByDateAsync(date, professionalId);
            return Ok(appointments);
        }

        // POST: /api/appointments
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AppointmentCreateDto dto)
        {
            try
            {
                var appointment = new Appointment
                {
                    ProfessionalId = dto.ProfessionalId,
                    ClientId = dto.ClientId,
                    StartAt = dto.StartAt,
                    EndAt = dto.EndAt,
                    Notes = dto.Notes,
                    Status = AppointmentStatus.Scheduled
                };

                var created = await _appointmentService.CreateAsync(appointment);
                return Ok(created); // o Created(...) si querés, pero Ok es aceptable en clase
            }
            catch (Exception ex)
            {
                // Mapeo simple por prefijo (de clase, sin middleware)
                if (ex.Message.StartsWith("PROFESSIONAL_NOT_FOUND"))
                    return NotFound(new { error = ex.Message });

                if (ex.Message.StartsWith("CLIENT_NOT_FOUND"))
                    return NotFound(new { error = ex.Message });

                if (ex.Message.StartsWith("OVERLAP"))
                    return Conflict(new { error = ex.Message });

                // INVALID_DATES u otros -> 400
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Cancel(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { error = "Appointment not found." });

            appointment.Status = AppointmentStatus.Canceled;
            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return NoContent();
        }


        [HttpPut("{id:int}/complete")]
        [Authorize(Roles = "Admin,SuperAdmin")]
        public async Task<IActionResult> Complete(int id)
        {
            var appointment = await _appointmentRepository.GetByIdAsync(id);
            if (appointment == null)
                return NotFound(new { error = "Appointment not found." });

            appointment.Status = AppointmentStatus.Completed;
            _appointmentRepository.Update(appointment);
            await _appointmentRepository.SaveChangesAsync();

            return NoContent();
        }

    }
}
