using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Interfaces.Repositories
{
    public interface IGroupRepository : IRepository<Group>
    {
        Task<IEnumerable<Group>> GetGroupsByUserAsync(Guid userId);
        Task<List<Group>> GetAllValidAsync();
    }
}
