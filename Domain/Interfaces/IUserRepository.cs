using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUsername(string username);
        User GetByEmail(string email);
        IEnumerable<User> GetBySkillLevel(int skillLevelId);
        IEnumerable<User> FindCompatiblePlayers(int userId, int skillLevelTolerance = 1);
    }
}
