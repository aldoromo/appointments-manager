using Appointments.Api.Attributes;
using Appointments.Application.DTOs;
using Appointments.Application.Interfaces;
using Appointments.Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IAppointmentService _appointmentService;

        public AppointmentsController(IAppointmentService appointmentService)
        {
            _appointmentService = appointmentService;
        }


        // GET: api/appointments
        //[HttpGet]
        //public async Task<IActionResult> GetAll()
        //{
        //    var appointment = await _appointmentService.GetAllAsync();
        //    return Ok(appointment);
        //}



        // GET: api/appointments/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var appointment = await _appointmentService.GetByIdAsync(id);
            if (appointment == null) return NotFound();
            return Ok(appointment);
        }

        // GET: api/appointments?from=2025-09-01&to=2025-09-10
        [HttpGet]
        public async Task<IActionResult> GetByRange([FromQuery] DateTime? from = null, [FromQuery] DateTime? to = null, [FromQuery] string? orderBy=null)
        {
            IEnumerable<AppointmentDto> appointment = default;
            if (from == null || to == null)
            {
                HttpContext.Items.TryGetValue("UserId", out var userIdObj);
                if (userIdObj == null || userIdObj is not int userId)
                    return Unauthorized("UserId not found in context");

                HttpContext.Items.TryGetValue("UserRole", out var userRol);
                orderBy ??= "date";
                if ((UserRole)userRol == UserRole.Manager)
                {
                    appointment = await _appointmentService.GetAllAsync(orderBy);
                }
                else {
                    appointment = await _appointmentService.GetByUserIdAsync(userId,orderBy);
                }
                return Ok(appointment);
            }
            appointment = await _appointmentService.GetByDateRangeAsync(from.Value, to.Value);
            return Ok(appointment);
        }

        // POST: api/appointments
        [HttpPost]
        public async Task<IActionResult> Create(AppointmentDto dto)
        {
            HttpContext.Items.TryGetValue("UserId", out var userIdObj);
            if (userIdObj == null || userIdObj is not int userId)
                return Unauthorized("UserId not found in context");

            var id = await _appointmentService.CreateAsync(dto,userId);
            return CreatedAtAction(nameof(GetById), new { id }, dto);
        }

        // PUT: api/appointments/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, AppointmentDto dto)
        {
            if (id != dto.AppointmentId) return BadRequest("AppointmentId mismatch");

            HttpContext.Items.TryGetValue("UserId", out var userIdObj);
            if (userIdObj == null || userIdObj is not int userId)
                return Unauthorized("UserId not found in context");

            await _appointmentService.UpdateAsync(dto, userId);
            return NoContent();
        }

        // PUT: api/appointments/5/approve
        [HttpPut("{id:int}/approve")]
        [AuthorizeRole((int)UserRole.Manager)]
        public async Task<IActionResult> Approve(int id)
        {
            HttpContext.Items.TryGetValue("UserId", out var userIdObj);
            if (userIdObj == null || userIdObj is not int userId)
                return Unauthorized("UserId not found in context");

            await _appointmentService.ApproveAsync(id,userId);
            return NoContent();
        }

        // PUT: api/appointments/5/cancel
        [HttpPut("{id:int}/cancel")]
        [AuthorizeRole((int)UserRole.Manager)]
        public async Task<IActionResult> Cancel(int id)
        {
            HttpContext.Items.TryGetValue("UserId", out var userIdObj);
            if (userIdObj == null || userIdObj is not int userId)
                return Unauthorized("UserId not found in context");

            await _appointmentService.CancelAsync(id, userId);
            return NoContent();
        }

        // DELETE: api/appointments/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            HttpContext.Items.TryGetValue("UserId", out var userIdObj);
            if (userIdObj == null || userIdObj is not int userId)
                return Unauthorized("UserId not found in context");

            await _appointmentService.DeleteAsync(id, userId);
            return NoContent();
        }
    }
}
