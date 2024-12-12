using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretFriendOrganizer.Application.DTOs
{
    public class CreateGroupRequest
    {
        public string GroupName { get; set; }
        public string Description { get; set; }
        public Guid CreatorUserId { get; set; }
    }
}
