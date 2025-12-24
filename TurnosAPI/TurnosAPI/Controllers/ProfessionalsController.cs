using Application.Services;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace TurnosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProfessionalsController : ControllerBase
    {
        private readonly ProfessionalService _service;

        public ProfessionalsController(ProfessionalService service)
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
            if (item == null) return NotFound(new { error = "Professional not found." });
            return Ok(item);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Professional professional)
        {
            try
            {
                var created = await _service.CreateAsync(professional);
                return Ok(created);
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] Professional professional)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, professional);
                return Ok(updated);
            }
            catch (Exception ex)
            {
                if (ex.Message == "PROFESSIONAL_NOT_FOUND")
                    return NotFound(new { error = "Professional not found." });

                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new { error = "Professional not found." });
            return NoContent();
        }
    }
}
