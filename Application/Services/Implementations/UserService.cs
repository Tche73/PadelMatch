using Application.DTO_s;
using Application.Queries;
using Application.Queries.Interfaces;
using Application.Services.Interfaces;
using Domain.Entities;
using Domain.Enums;
using Domain.Interface.Repositories;
using Domain.Interfaces;

namespace Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IQueryHandler<FindCompatiblePlayersQuery, IEnumerable<UserDto>> _findCompatiblePlayersQueryHandler;

        public UserService(IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, IQueryHandler<FindCompatiblePlayersQuery, IEnumerable<UserDto>> findCompatiblePlayersQueryHandler)
        {
            _unitOfWork = unitOfWork;
            _passwordHasher = passwordHasher;
            _findCompatiblePlayersQueryHandler = findCompatiblePlayersQueryHandler;
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

        public void Create(User user, string password, UserRole role = UserRole.User)
        {
            // Vérifier que l'utilisateur n'existe pas déjà
            if (_unitOfWork.Users.GetByUsername(user.Username) != null)
                throw new InvalidOperationException("Ce nom d'utilisateur est déjà pris.");

            if (_unitOfWork.Users.GetByEmail(user.Email) != null)
                throw new InvalidOperationException("Cet email est déjà utilisé.");

            // Hacher le mot de passe et l'assigner à PasswordHash
            user.PasswordHash = _passwordHasher.HashPassword(password);
            user.CreatedAt = DateTime.Now;
            user.IsActive = true;
            user.Role = role;

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

            existingUser.Email = user.Email;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.SkillLevelId = user.SkillLevelId;

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();
        }

        public void UpdatePassword(int userId, string newPassword)
        {
            var user = _unitOfWork.Users.GetById(userId);
            if (user == null)
                throw new InvalidOperationException("Utilisateur non trouvé.");

            // Hacher le nouveau mot de passe
            user.PasswordHash = _passwordHasher.HashPassword(newPassword);

            _unitOfWork.Users.Update(user);
            _unitOfWork.Complete();
        }

        public IEnumerable<UserDto> FindCompatiblePlayers(int userId, int skillLevelTolerance = 1)
        {
            var query = new FindCompatiblePlayersQuery
            {
                UserId = userId,
                SkillLevelTolerance = skillLevelTolerance
            };

            return _findCompatiblePlayersQueryHandler.Handle(query);
        }

        // Ajoutez cette méthode à votre classe UserService existante
        //public IEnumerable<User> SearchPlayers(PlayerSearchRequestDto request)
        //{
        //    var query = _unitOfWork.Users.GetAll().Where(u => u.IsActive).AsQueryable();

        //    // Filtrer par niveau de compétence si spécifié
        //    if (request.SkillLevelId.HasValue)
        //    {
        //        int minLevel = request.SkillLevelId.Value - (request.SkillLevelTolerance ?? 1);
        //        int maxLevel = request.SkillLevelId.Value + (request.SkillLevelTolerance ?? 1);

        //        // S'assurer que les niveaux ne sont pas négatifs
        //        minLevel = Math.Max(1, minLevel);

        //        query = query.Where(u => u.SkillLevelId >= minLevel && u.SkillLevelId <= maxLevel);
        //    }

        //    // Exclure l'utilisateur actuel
        //    if (request.CurrentUserId.HasValue)
        //    {
        //        query = query.Where(u => u.Id != request.CurrentUserId.Value);
        //    }

        //    // Récupérer les utilisateurs filtrés
        //    var filteredUsers = query.ToList();

        //    // Si des critères de disponibilité sont spécifiés, filtrer davantage
        //    if (request.DayOfWeek.HasValue || request.StartTime.HasValue || request.EndTime.HasValue)
        //    {
        //        filteredUsers = filteredUsers.Where(u =>
        //            HasCompatibleAvailability(u.Id, request.DayOfWeek, request.StartTime, request.EndTime)).ToList();
        //    }

        //    return filteredUsers;
        //}

        //private bool HasCompatibleAvailability(int userId, int? dayOfWeek, TimeSpan? startTime, TimeSpan? endTime)
        //{
        //    // Cette méthode vérifie si l'utilisateur a des disponibilités compatibles
        //    var availabilities = _unitOfWork.Availabilities.GetByUserId(userId);

        //    if (!dayOfWeek.HasValue && !startTime.HasValue && !endTime.HasValue)
        //        return true;

        //    // Si seul le jour est spécifié, vérifier uniquement le jour
        //    if (dayOfWeek.HasValue && !startTime.HasValue && !endTime.HasValue)
        //    {
        //        return availabilities.Any(a => a.DayOfWeek == dayOfWeek.Value);
        //    }

        //    // Si le jour et au moins une heure sont spécifiés
        //    return availabilities.Any(a =>
        //        (!dayOfWeek.HasValue || a.DayOfWeek == dayOfWeek.Value) &&
        //        (
        //            // Si les deux heures sont spécifiées, vérifier le chevauchement
        //            (startTime.HasValue && endTime.HasValue &&
        //             ((a.StartTime <= startTime.Value && a.EndTime > startTime.Value) ||
        //              (a.StartTime < endTime.Value && a.EndTime >= endTime.Value) ||
        //              (a.StartTime >= startTime.Value && a.EndTime <= endTime.Value))) ||

        //            // Si seulement l'heure de début est spécifiée
        //            (startTime.HasValue && !endTime.HasValue && a.EndTime > startTime.Value) ||

        //            // Si seulement l'heure de fin est spécifiée
        //            (!startTime.HasValue && endTime.HasValue && a.StartTime < endTime.Value)
        //        ));
        public IEnumerable<User> SearchPlayers(PlayerSearchRequestDto request)
        {
            Console.WriteLine($"SearchPlayers appelé avec DayOfWeek={request.DayOfWeek}");

            var query = _unitOfWork.Users.GetAll().Where(u => u.IsActive).AsQueryable();

            // Filtrer par niveau de compétence si spécifié
            if (request.SkillLevelId.HasValue)
            {
                int minLevel = request.SkillLevelId.Value - (request.SkillLevelTolerance ?? 1);
                int maxLevel = request.SkillLevelId.Value + (request.SkillLevelTolerance ?? 1);

                // S'assurer que les niveaux ne sont pas négatifs
                minLevel = Math.Max(1, minLevel);

                query = query.Where(u => u.SkillLevelId >= minLevel && u.SkillLevelId <= maxLevel);
            }

            // Exclure l'utilisateur actuel
            if (request.CurrentUserId.HasValue)
            {
                query = query.Where(u => u.Id != request.CurrentUserId.Value);
            }

            // Récupérer les utilisateurs filtrés
            var filteredUsers = query.ToList();
            Console.WriteLine($"Après filtrage initial : {filteredUsers.Count} utilisateurs");

            // Si des critères de disponibilité sont spécifiés, filtrer davantage
            if (request.DayOfWeek.HasValue || request.StartTime.HasValue || request.EndTime.HasValue)
            {
                var usersBeforeAvailabilityFilter = filteredUsers.Count;

                filteredUsers = filteredUsers.Where(u =>
                    HasCompatibleAvailability(u.Id, request.DayOfWeek, request.StartTime, request.EndTime)).ToList();

                Console.WriteLine($"Après filtrage par disponibilité : {filteredUsers.Count} utilisateurs (supprimés: {usersBeforeAvailabilityFilter - filteredUsers.Count})");
            }

            return filteredUsers;
        }

        private bool HasCompatibleAvailability(int userId, int? dayOfWeek, TimeSpan? startTime, TimeSpan? endTime)
        {
            // Cette méthode vérifie si l'utilisateur a des disponibilités compatibles
            var availabilities = _unitOfWork.Availabilities.GetByUserId(userId);
            Console.WriteLine($"User {userId} a {availabilities.Count()} disponibilités");

            if (!dayOfWeek.HasValue && !startTime.HasValue && !endTime.HasValue)
                return true;

            // Si seul le jour est spécifié, vérifier uniquement le jour
            if (dayOfWeek.HasValue)
            {
                var hasMatchingDay = availabilities.Any(a => a.DayOfWeek == dayOfWeek.Value);
                Console.WriteLine($"User {userId} - Jour {dayOfWeek.Value} - Match: {hasMatchingDay}");

                // Pour debuggage, afficher toutes les disponibilités
                foreach (var a in availabilities)
                {
                    Console.WriteLine($"  Disponibilité: Jour {a.DayOfWeek}, {a.StartTime}-{a.EndTime}");
                }

                return hasMatchingDay;
            }

            // Gestion du cas où seules les heures sont spécifiées
            return availabilities.Any(a =>
                (!startTime.HasValue || a.StartTime <= startTime.Value) &&
                (!endTime.HasValue || a.EndTime >= endTime.Value));
        }    
    }
}
