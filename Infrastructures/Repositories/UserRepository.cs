using Domain.Entities;
using Infrastructures.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using BStorm.Tools.Database;
using Domain.Interfaces;
namespace Infrastructures.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        
        public UserRepository(PadelMatchDbContext context) : base(context)
        {
        }

        public User GetByEmail(string email)
        {
            return _dbSet.Include(u => u.SkillLevel)
                .FirstOrDefault(u => u.Email == email);
        }

        public IEnumerable<User> GetBySkillLevel(int skillLevelId)
        {
            return _dbSet.Include(u => u.SkillLevel)
                .Where(u => u.SkillLevelId == skillLevelId).ToList();
        }

        public User GetByUsername(string username)
        {
            return _dbSet.Include(u => u.SkillLevel)
                .FirstOrDefault(u => u.Username == username);
        }

       
        public IEnumerable<User> FindCompatiblePlayers(int userId, int skillLevelTolerance = 1)
        {
            // Utilisez l'extension que vous avez mentionnée de BStorm.Tools.Database
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();

                return connection.ExecuteReader(
                    "sp_FindCompatiblePlayers",
                    record => new User
                    {
                        Id = (int)record["Id"],
                        Username = (string)record["Username"],
                        Email = (string)record["Email"],
                        FirstName = (string)record["FirstName"],
                        LastName = record["LastName"] == DBNull.Value ? null : (string)record["LastName"],
                        SkillLevelId = (int)record["SkillLevelId"],
                        CreatedAt = (DateTime)record["CreatedAt"],
                        IsActive = (bool)record["IsActive"]
                    },
                    isStoredProcedure: true,
                    parameters: new { UserId = userId, SkillLevelTolerance = skillLevelTolerance }
                );
            }
        }


        public override IEnumerable<User> GetAll()
        {
            return _dbSet.Include(u => u.SkillLevel).ToList();
        }

        public override User GetById(int id)
        {
            return _dbSet.Include(u => u.SkillLevel)
                .FirstOrDefault(u => u.Id == id);
        }
    }
}
