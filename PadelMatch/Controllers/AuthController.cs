using Application.Services.Implementations;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using PadelMatch.Models.Requests;
using PadelMatch.Models.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace PadelMatch.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IJwtService _jwtService;

        public AuthController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public ActionResult<AuthResponse> Login(LoginRequest request)
        {
            if (string.IsNullOrEmpty(request.Username) || string.IsNullOrEmpty(request.Password))
                return BadRequest(new { message = "Nom d'utilisateur et mot de passe requis" });

            var isAuthenticated = _userService.Authenticate(request.Username, request.Password);
            if (!isAuthenticated)
                return Unauthorized(new { message = "Nom d'utilisateur ou mot de passe incorrect" });

            var user = _userService.GetByUsername(request.Username);
            var token = _jwtService.GenerateToken(user);

            return Ok(new AuthResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Token = token
            });
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public ActionResult Register(RegisterUserRequest request)
        {
            try
            {
                // Validation supplémentaire si nécessaire
                if (_userService.GetByUsername(request.Username) != null)
                    return BadRequest(new { message = "Ce nom d'utilisateur existe déjà" });

                if (_userService.GetByEmail(request.Email) != null)
                    return BadRequest(new { message = "Cet email est déjà utilisé" });

                // Utilisez la méthode Create du service qui devrait s'occuper du hachage
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    SkillLevelId = request.SkillLevelId,
                    Role = UserRole.User // Toujours User par défaut
                };

                _userService.Create(user, request.Password);

                return Ok(new { message = "Utilisateur créé avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }   
    }
}
