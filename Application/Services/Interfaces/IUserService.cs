using Domain.Entities;
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
        void Create(User user);
        void Update(User user);
        void Delete(int id);
        bool Authenticate(string username, string password);
    }
}
