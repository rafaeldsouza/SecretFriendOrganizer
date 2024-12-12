namespace SecretFriendOrganizer.Application.DTOs
{
    public class ValidGroupDto
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
    }
}
