using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Interfaces.Repositories
{
    public interface IGroupMembershipRepository : IRepository<GroupMembership>
    {
        Task<IEnumerable<GroupMembership>> GetByGroupIdAsync(Guid groupId);
        Task<GroupMembership?> GetByUserIdAndGroupIdAsync(Guid userId, Guid groupId);
        Task<List<GroupMembership>> GetAllByUserAsync(Guid userId);
    }
}