using Microsoft.EntityFrameworkCore;
using SecretFriendOrganizer.Domain.Entities;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Infrastructure.Persistence;

namespace SecretFriendOrganizer.Infrastructure.Repositories
{
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        public GroupRepository(ApplicationDbContext context) : base(context) { }

        public async Task<IEnumerable<Group>> GetGroupsByUserAsync(Guid userId)
        {
            return await _context.Groups
                .Where(g => g.Members.Any(m => m.UserId == userId))
                .ToListAsync();
        }

        public async Task<List<Group>> GetAllValidAsync()
        {
            return await _context.Groups
                .Include(g => g.CreatedBy)
                .Where(g => !g.IsDrawn)
                .ToListAsync();
        }

        public override async Task<Group?> GetByIdAsync(Guid id)
        {
            return await _context.Groups.Include(g => g.Members).FirstAsync(x => x.Id == id);
        }
    }
}
