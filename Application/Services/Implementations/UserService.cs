using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
        }

        public bool Authenticate(string username, string password)
        {
           var user = _unitOfWork.Users.GetByUsername(username);
            if (user == null)
            {
                return false;
            }

            if (!user.IsActive)
                return false;

            return _passwordHasher.VerifyPassword(password, user.PasswordHash);
        }

        public void Create(User user)
        {
            // Vérifier que l'utilisateur n'existe pas déjà
            if (_unitOfWork.Users.GetByUsername(user.Username) != null)
                throw new InvalidOperationException("Ce nom d'utilisateur est déjà pris.");

            if (_unitOfWork.Users.GetByEmail(user.Email) != null)
                throw new InvalidOperationException("Cet email est déjà utilisé.");

            // Hacher le mot de passe
            user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;

            _unitOfWork.Users.Add(user);
            _unitOfWork.Complete();
        }

        public void Delete(int id)
        {
            var user = _unitOfWork.Users.GetById(id);
            if (user == null)
            {
                throw new InvalidOperationException("Utilisateur non trouvé");
            }

            // Soft delete - marquer comme inactif plutôt que de supprimer
            user.IsActive = false;
            _unitOfWork.Complete();
        }

        public IEnumerable<User> GetAll()
        {
            return _unitOfWork.Users.GetAll();
        }

        public User GetByEmail(string email)
        {
            return _unitOfWork.Users.GetByEmail(email);
        }

        public User GetById(int id)
        {
           return _unitOfWork.Users.GetById(id);
        }

        public IEnumerable<User> GetBySkillLevel(int skillLevelId)
        {
            return _unitOfWork.Users.GetBySkillLevel(skillLevelId);
        }

        public User GetByUsername(string username)
        {
            return _unitOfWork.Users.GetByUsername(username);
        }

        public void Update(User user)
        {
            // Vérification que l'utilisateur existe
            var existingUser = _unitOfWork.Users.GetById(user.Id);
            if (existingUser == null)
                throw new InvalidOperationException("Utilisateur non trouvé.");

            // Vérifier si le nom d'utilisateur est déjà utilisé par un autre utilisateur
            var userWithSameUsername = _unitOfWork.Users.GetByUsername(user.Username);
            if (userWithSameUsername != null && userWithSameUsername.Id != user.Id)
                throw new InvalidOperationException("Ce nom d'utilisateur est déjà pris.");

            // Vérifier si l'email est déjà utilisé par un autre utilisateur
            var userWithSameEmail = _unitOfWork.Users.GetByEmail(user.Email);
            if (userWithSameEmail != null && userWithSameEmail.Id != user.Id)
                throw new InvalidOperationException("Cet email est déjà utilisé.");

            // Ne pas écraser le mot de passe existant si aucun nouveau n'est fourni
            if (string.IsNullOrEmpty(user.PasswordHash))
            {
                user.PasswordHash = existingUser.PasswordHash;
            }
            else if (user.PasswordHash != existingUser.PasswordHash)
            {
                // Si un nouveau mot de passe a été fourni, le hacher
                user.PasswordHash = _passwordHasher.HashPassword(user.PasswordHash);
            }

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();
        }

    }
}
