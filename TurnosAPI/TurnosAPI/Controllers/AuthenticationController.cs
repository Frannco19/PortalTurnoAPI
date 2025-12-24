using Application.Interfaces;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace TurnosAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ICustomAuthenticationService _authService;

        public AuthenticationController(ICustomAuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public ActionResult<string> Login([FromBody] LoginRequest loginRequest)
        {
            var token = _authService.Login(loginRequest);
            if (string.IsNullOrEmpty(token))
                return Unauthorized();

            return Ok(token);
        }
    }
}
