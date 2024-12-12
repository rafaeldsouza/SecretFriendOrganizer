namespace SecretFriendOrganizer.Domain.Entities
{
    public class GroupMembership
    {
        public Guid Id { get; set; } 
        public Guid GroupId { get; set; } 
        public Guid UserId { get; set; } 
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public bool IsAdmin { get; set; } = false;

        public Group Group { get; set; } = null!;
        public User User { get; set; } = null!;
       
        public string? GiftRecommendation { get; set; } 
        public Guid? AssignedFriendId { get; set; } 
        public User? AssignedFriend { get; set; } 
    }
}
