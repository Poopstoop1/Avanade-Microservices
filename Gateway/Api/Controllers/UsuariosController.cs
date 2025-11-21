using Api.DTOs;
using Api.Interfaces;
using Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioRepository _repo;
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;

        public UsuariosController(IUsuarioRepository repo, AuthService authservice, JwtService jwtService)
        {
            _repo = repo;
            _authService = authservice;
            _jwtService = jwtService;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _authService.Register(dto.Name, dto.Email, dto.Role, dto.Password, cancellationToken);

                return Ok(new { user.Name, user.Email });
            }
            catch (Exception ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] ILoginDTO dto, CancellationToken cancellationToken)
        {
            try { 
                var user = await _authService.Login(dto.Email, dto.Password, cancellationToken);

                if(user == null) return Unauthorized();

                var token = _jwtService.GenerateJwt(user);
                return Ok(new { token });
            }
            catch (Exception ex)
            {
                return Unauthorized(new { message = ex.Message });
            }
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsuarios(CancellationToken cancellationToken)
        {
            var usuarios = await _repo.GetAllUsuarioAsync(cancellationToken);
            return Ok(usuarios);
        }

    }
}
