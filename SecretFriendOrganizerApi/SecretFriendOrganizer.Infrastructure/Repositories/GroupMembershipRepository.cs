using Microsoft.EntityFrameworkCore;
using SecretFriendOrganizer.Domain.Entities;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Infrastructure.Persistence;

namespace SecretFriendOrganizer.Infrastructure.Repositories
{
    public class GroupMembershipRepository : Repository<GroupMembership>, IGroupMembershipRepository
    {
        public GroupMembershipRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<GroupMembership>> GetByGroupIdAsync(Guid groupId)
        {
            return await _context.GroupMemberships
                .Where(gm => gm.GroupId == groupId)
                .ToListAsync();
        }

        public async Task<GroupMembership?> GetByUserIdAndGroupIdAsync(Guid userId, Guid groupId)
        {
            return await _context.GroupMemberships.Include(x=>x.Group).Include(x=>x.AssignedFriend)
               .FirstOrDefaultAsync(gm => gm.UserId == userId && gm.GroupId == groupId) ;
        }     

        public async Task<GroupMembership?> GetByUserAsync(Guid userId)
        {
            return await _context.GroupMemberships
                .FirstOrDefaultAsync(m => m.UserId == userId);
        }

        public async Task<List<GroupMembership>> GetAllByUserAsync(Guid userId)
        {
            return await _context.GroupMemberships
                .Where(m => m.UserId == userId)
                .Include(m => m.Group)
                .ToListAsync();
        }
    }
}

