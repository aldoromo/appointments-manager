using Appointments.Application.DTOs;
using Appointments.Application.Interfaces;
using Appointments.Models;
using Microsoft.AspNetCore.Mvc;

namespace Appointments.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        // GET: api/users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
        {
            var users = await _userService.GetAllAsync();
            return Ok(users);
        }

        // GET: api/users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UserDto>> GetById(string id)
        {
            var user = default(UserDto);
            if (!int.TryParse(id, out int userId))
            {
                user = await _userService.GetByUsernameAsync(id);
                if (user == null) return NotFound();
                return Ok(user);

            }
            user = await _userService.GetByIdAsync(userId);
            if (user == null) return NotFound();
            return Ok(user);
        }

        // POST: api/users
        [HttpPost]
        public async Task<ActionResult<UserDto>> Create(UserDto dto)
        {
            var id = await _userService.CreateAsync(dto);
            dto.UserId = id;

            return CreatedAtAction(nameof(GetById), new { id = dto.UserId }, dto);
        }

        // PUT: api/users/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, UserDto dto)
        {
            if (id != dto.UserId) return BadRequest("Id mismatch");

            await _userService.UpdateAsync(dto, id);
            return NoContent();
        }

        // DELETE: api/users/5
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return NoContent();
        }
    }
}
