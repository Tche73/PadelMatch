using System.ComponentModel.DataAnnotations;

namespace PadelMatchBlazor.Models
{
    public class RegisterRequest
    {
        [Required(ErrorMessage = "Le prénom est requis")]
        [StringLength(50, ErrorMessage = "Le prénom doit faire entre {2} et {1} caractères", MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Le nom est requis")]
        [StringLength(50, ErrorMessage = "Le nom doit faire entre {2} et {1} caractères", MinimumLength = 2)]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Le nom d'utilisateur est requis")]
        [StringLength(50, ErrorMessage = "Le nom d'utilisateur doit faire entre {2} et {1} caractères", MinimumLength = 3)]
        public string Username { get; set; }

        [Required(ErrorMessage = "L'email est requis")]
        [EmailAddress(ErrorMessage = "Format d'email invalide")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Le mot de passe est requis")]
        [StringLength(100, ErrorMessage = "Le mot de passe doit faire entre {2} et {1} caractères", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "La confirmation du mot de passe est requise")]
        [Compare("Password", ErrorMessage = "Les mots de passe ne correspondent pas")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Le niveau de jeu est requis")]
        public int SkillLevelId { get; set; }
    }
}
