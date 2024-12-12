using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecretFriendOrganizer.Application.DTOs
{
    public class JoinGroupRequest
    {
        public Guid GroupId { get; set; } 
        public Guid UserId { get; set; } 
        public string GiftRecommendation { get; set; } 
    }
}
