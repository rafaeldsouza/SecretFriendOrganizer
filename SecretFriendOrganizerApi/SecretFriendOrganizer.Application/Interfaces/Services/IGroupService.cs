using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Interfaces.Services
{
    public interface IGroupService
    {
        Task<ServiceResponse<Group>> CreateGroupAsync(string groupName, string description, Guid userId);   
        Task<ServiceResponse<string>> GetAssignedFriendAsync(Guid userId, Guid groupId);       
        Task<ServiceResponse<List<UserGroupDto>>> ListUserGroupsAsync(Guid userId);
        Task<ServiceResponse> PerformDrawAsync(Guid groupId, Guid adminUserId);

        Task<ServiceResponse<List<ValidGroupDto>>> GetAllValidGroupsAsync();

        Task<ServiceResponse> AddUserToGroupAsync(Guid groupId, Guid userId, string giftRecommendation);
    }
}
