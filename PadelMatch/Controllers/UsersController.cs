using Application.DTO_s;
using Application.Services.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PadelMatch.Models.Requests;
using PadelMatch.Models.Responses;
using System.Linq;
using System.Security.Claims;

namespace PadelMatch.Controllers
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

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<UserResponse>> GetAllUsers()
        {
            var users = _userService.GetAll();
            var userResponses = users.Select(MapToResponse);
            return Ok(userResponses);
        }

        [HttpGet("{id}")]
        [Authorize]
        public ActionResult<UserResponse> GetUserById(int id)
        {
            var user = _userService.GetById(id);
            if (user == null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        [HttpGet("me")]
        [Authorize]
        public ActionResult<UserResponse> GetCurrentUserProfile()
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var user = _userService.GetById(currentUserId);

            if (user == null)
                return NotFound();

            return Ok(MapToResponse(user));
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public ActionResult CreateUserByAdmin(RegisterUserByAdminRequest request)
        {
            try
            {
                var user = new User
                {
                    Username = request.Username,
                    Email = request.Email,
                    PasswordHash = request.Password, // Sera haché par le service
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    SkillLevelId = request.SkillLevelId,
                    Role = request.Role
                };

                _userService.Create(user, request.Password, request.Role);
                return Ok(new { message = "Utilisateur créé avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpPut("{id}")]
        [Authorize]
        public ActionResult UpdateUser(int id, UpdateUserRequest request)
        {
            // Vérifier que l'utilisateur modifie son propre profil ou est admin
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            var isAdmin = User.IsInRole("Admin");

            if (id != currentUserId && !isAdmin)
                return Forbid();

            try
            {
                var user = _userService.GetById(id);
                if (user == null)
                    return NotFound();

                user.Email = request.Email;
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.SkillLevelId = request.SkillLevelId;

                // Ne mettre à jour le mot de passe que s'il est fourni
                if (!string.IsNullOrEmpty(request.Password))
                {
                    _userService.UpdatePassword(id, request.Password);
                    //user.PasswordHash = request.Password;
                }

                _userService.Update(user);
                return Ok(new { message = "Utilisateur mis à jour avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteUser(int id)
        {
            try
            {
                _userService.Delete(id);
                return Ok(new { message = "Utilisateur désactivé avec succès" });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("compatible")]
        [Authorize]
        public ActionResult<IEnumerable<UserResponse>> FindCompatiblePlayers([FromQuery] int userId, [FromQuery] int skillLevelTolerance = 1)
        {
            var userDtos = _userService.FindCompatiblePlayers(userId, skillLevelTolerance);
            var userResponses = userDtos.Select(MapDtoToResponse);
            return Ok(userResponses);
        }


        [HttpGet("search")]
        [Authorize]
        public ActionResult<IEnumerable<UserResponse>> SearchPlayers([FromQuery] PlayerSearchRequestDto request)
        {
            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);

            // Ajouter l'ID de l'utilisateur actuel si non spécifié
            if (request.CurrentUserId == null)
            {
                request.CurrentUserId = currentUserId;
            }

            var users = _userService.SearchPlayers(request);
            var userResponses = users.Select(MapToResponse);
            return Ok(userResponses);
        }
        private UserResponse MapDtoToResponse(UserDto userDto)
        {
            return new UserResponse
            {
                Id = userDto.Id,
                Username = userDto.Username,
                Email = userDto.Email,
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                SkillLevelId = userDto.SkillLevelId,
                SkillLevelName = userDto.SkillLevelName
            };
        }

        private UserResponse MapToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                SkillLevelId = user.SkillLevelId,
                SkillLevelName = user.SkillLevel?.Name,
                IsActive = user.IsActive,
                CreatedAt = user.CreatedAt,
            };
        }
    }
}
