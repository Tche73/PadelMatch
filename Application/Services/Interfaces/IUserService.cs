using Application.DTO_s;
using Domain.Entities;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services.Interfaces
{
    public interface IUserService
    {
        User GetById(int id);
        User GetByUsername(string username);
        User GetByEmail(string email);
        IEnumerable<User> GetAll();
        IEnumerable<User> GetBySkillLevel(int skillLevelId);
        void Create(User user, string password, UserRole role = UserRole.User);
        void Update(User user);
        void Delete(int id);
        bool Authenticate(string username, string password);
        void UpdatePassword(int userid, string newPassword);
        IEnumerable<UserDto> FindCompatiblePlayers(int userId, int skillLevelTolerance = 1);
        IEnumerable<User> SearchPlayers(PlayerSearchRequestDto request);
        IEnumerable<User> GetActiveUsers();
    }
}
