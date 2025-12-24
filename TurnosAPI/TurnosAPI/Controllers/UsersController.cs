using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TurnosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly UserService _service;

        public UsersController(UserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
            => Ok(await _service.GetAllAsync());

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new { error = "User not found." });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] User user)
        {
            try
            {
                var created = await _service.CreateAsync(user);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] User user)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, user);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                if (ex.Message == "USER_NOT_FOUND")
                    return NotFound(new { error = "User not found." });

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "User not found." });
            return NoContent();
        }
    }
}
