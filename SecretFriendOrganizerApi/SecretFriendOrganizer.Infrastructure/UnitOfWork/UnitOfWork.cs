using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Infrastructure.Persistence;

namespace SecretFriendOrganizer.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context, IGroupRepository groupRepository, IUserRepository userRepository, IGroupMembershipRepository groupMembershipRepository)
        {
            _context = context;
            Groups = groupRepository;
            Users = userRepository;
            GroupMemberships = groupMembershipRepository;
        }

        public IGroupRepository Groups { get; }
        public IUserRepository Users { get; }
        public IGroupMembershipRepository GroupMemberships { get; }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }

}
