using System.Text.RegularExpressions;

namespace SecretFriendOrganizer.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string KeycloakId { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Group> Groups { get; set; } = new List<Group>();
        public ICollection<GroupMembership> Memberships { get; set; } = new List<GroupMembership>();
    }
}
