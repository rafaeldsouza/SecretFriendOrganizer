using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretFriendOrganizer.Application.DTOs
{
    public class UserGroupDto
    {
        public Guid GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsDrawn { get; set; } 
        public bool IsAdmin { get; set; } 
    }
}
