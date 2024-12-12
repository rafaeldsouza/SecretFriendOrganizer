namespace SecretFriendOrganizer.Application.Interfaces.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        IGroupRepository Groups { get; }
        IUserRepository Users { get; }
        IGroupMembershipRepository GroupMemberships { get; }
        Task<int> SaveChangesAsync();
    }
}
