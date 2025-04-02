using Domain.Entities;
using Domain.Interface.Repositories;
using Infrastructures.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructures.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return _dbSet.FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetBySkillLevel(int skillLevelId)
        {
            return _dbSet.Where(u => u.SkillLevelId == skillLevelId).ToList();
        }

        public User GetByUsername(string username)
        {
            return _dbSet.FirstOrDefault(u => u.Username == username);
        }
    }
}
