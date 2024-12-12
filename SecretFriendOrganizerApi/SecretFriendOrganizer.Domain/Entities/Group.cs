namespace SecretFriendOrganizer.Domain.Entities
{
    public class Group
    {
        public Guid Id { get; set; } 
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public Guid CreatedById { get; set; } 

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsDrawn { get; set; } = false; 
       
        public User CreatedBy { get; set; } = null!;
        public ICollection<GroupMembership> Members { get; set; } = new List<GroupMembership>();
    }
}
