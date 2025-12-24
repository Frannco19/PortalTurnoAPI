using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TurnosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ClientsController : ControllerBase
    {
        private readonly ClientService _service;

        public ClientsController(ClientService service)
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
            if (item == null) return NotFound(new { error = "Client not found." });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Client client)
        {
            try
            {
                var created = await _service.CreateAsync(client);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Client client)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, client);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                if (ex.Message == "CLIENT_NOT_FOUND")
                    return NotFound(new { error = "Client not found." });

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Client not found." });
            return NoContent();
        }
    }
}
