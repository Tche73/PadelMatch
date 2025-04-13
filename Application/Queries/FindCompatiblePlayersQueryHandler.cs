using Application.DTO_s;
using Application.Queries.Interfaces;
using Domain.Interfaces;

namespace Application.Queries
{
    public class FindCompatiblePlayersQueryHandler : IQueryHandler<FindCompatiblePlayersQuery, IEnumerable<UserDto>>
    {
        private readonly IUnitOfWork _unitOfWork;

        public FindCompatiblePlayersQueryHandler(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<UserDto> Handle(FindCompatiblePlayersQuery query)
        {
            // Logique pour trouver des joueurs compatibles
            var currentUser = _unitOfWork.Users.GetById(query.UserId);
            if (currentUser == null)
                return Enumerable.Empty<UserDto>();

            // Recherche des joueurs avec un niveau de compétence proche
            var compatiblePlayers = _unitOfWork.Users.GetAll()
                .Where(u =>
                    u.Id != query.UserId &&
                    u.IsActive &&
                    Math.Abs(u.SkillLevelId - currentUser.SkillLevelId) <= query.SkillLevelTolerance)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Email = u.Email,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    SkillLevelId = u.SkillLevelId
                })
                .ToList();

            return compatiblePlayers;
        }
    }
}
