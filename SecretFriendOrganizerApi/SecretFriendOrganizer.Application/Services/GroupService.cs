using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces;
using SecretFriendOrganizer.Application.Interfaces.Repositories;
using SecretFriendOrganizer.Application.Interfaces.Services;
using SecretFriendOrganizer.Domain.Entities;

namespace SecretFriendOrganizer.Application.Services
{
    public class GroupService : IGroupService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<GroupService> _logger;

        public GroupService(IUnitOfWork unitOfWork, ILogger<GroupService> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<ServiceResponse<Group>> CreateGroupAsync(string groupName, string description, Guid userId)
        {
            try
            {
                // Criar o grupo
                var group = new Group
                {
                    Id = Guid.NewGuid(),
                    Name = groupName,
                    Description = description,
                    CreatedAt = DateTime.UtcNow,
                    CreatedById = userId
                };

                var groupMembership = new GroupMembership
                {
                    UserId = userId,
                    GroupId = group.Id,
                    IsAdmin = true
                };

                group.Members.Add(groupMembership);

                await _unitOfWork.Groups.AddAsync(group);
                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Group created successfully.");

                return new ServiceResponse<Group>
                {
                    Success = true,
                    Message = "Group created successfully.",
                    Data = group
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating group.");
                return new ServiceResponse<Group>
                {
                    Success = false,
                    Message = "Error creating group."
                };
            }
        }

        public async Task<ServiceResponse> AddUserToGroupAsync(Guid groupId, Guid userId, string giftRecommendation)
        {
            try
            {
                var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                if (group == null)
                {
                    return new ServiceResponse { Success = false, Message = "Group not found." };
                }

                if (group.IsDrawn)
                {
                    return new ServiceResponse { Success = false, Message = "The group is closed for new members." };
                }

                var existingMembership = await _unitOfWork.GroupMemberships.GetByUserIdAndGroupIdAsync(userId, groupId);
                if (existingMembership != null)
                {
                    return new ServiceResponse { Success = false, Message = "User is already a member of the group." };
                }

                var groupMembership = new GroupMembership
                {
                    GroupId = groupId,
                    UserId = userId,
                    GiftRecommendation = giftRecommendation,
                    IsAdmin = false
                };

                group.Members.Add(groupMembership);

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("User added to group successfully.");

                return new ServiceResponse { Success = true, Message = "User added to group successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user to group.");
                return new ServiceResponse { Success = false, Message = "Error adding user to group." };
            }
        }

        public async Task<ServiceResponse<string>> GetAssignedFriendAsync(Guid userId, Guid groupId)
        {
            try
            {
                var groupMembership = await _unitOfWork.GroupMemberships.GetByUserIdAndGroupIdAsync(userId, groupId);
                if (groupMembership == null)
                {
                    return new ServiceResponse<string> { Success = false, Message = "User is not part of this group." };
                }

                if (!groupMembership.Group.IsDrawn)
                {
                    return new ServiceResponse<string> { Success = false, Message = "The draw has not been completed yet." };
                }

                var assignedFriend = groupMembership.AssignedFriend;
                if (assignedFriend == null)
                {
                    return new ServiceResponse<string> { Success = false, Message = "No assigned friend found." };
                }

                _logger.LogInformation("Assigned friend retrieved successfully.");

                return new ServiceResponse<string>
                {
                    Success = true,
                    Message = "Assigned friend retrieved successfully.",
                    Data = $"Your Assigned friend is {assignedFriend.Username} and your gift recommendation is {(string.IsNullOrWhiteSpace(groupMembership.GiftRecommendation)? "Not informed": groupMembership.GiftRecommendation)}",                    
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting assigned friend.");
                return new ServiceResponse<string>
                {
                    Success = false,
                    Message = "Error getting assigned friend."
                };
            }
        }

        public async Task<ServiceResponse<List<UserGroupDto>>> ListUserGroupsAsync(Guid userId)
        {
            try
            {
                var groupMemberships = await _unitOfWork.GroupMemberships.GetAllByUserAsync(userId);
                if (groupMemberships == null || !groupMemberships.Any())
                {
                    return new ServiceResponse<List<UserGroupDto>>
                    {
                        Success = false,
                        Message = "User is not part of any groups."
                    };
                }

                var userGroups = groupMemberships.Select(m => new UserGroupDto
                {
                    GroupId = m.Group.Id,
                    GroupName = m.Group.Name,
                    IsDrawn = m.Group.IsDrawn,
                    IsAdmin = m.IsAdmin
                }).ToList();

                _logger.LogInformation("User groups retrieved successfully.");

                return new ServiceResponse<List<UserGroupDto>>
                {
                    Success = true,
                    Message = "User groups retrieved successfully.",
                    Data = userGroups
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error listing user groups.");
                return new ServiceResponse<List<UserGroupDto>>
                {
                    Success = false,
                    Message = "Error listing user groups."
                };
            }
        }

        public async Task<ServiceResponse> PerformDrawAsync(Guid groupId, Guid adminUserId)
        {
            try
            {
                var group = await _unitOfWork.Groups.GetByIdAsync(groupId);
                if (group == null)
                {
                    return new ServiceResponse { Success = false, Message = "Group not found." };
                }

                var adminMembership = group.Members.FirstOrDefault(m => m.UserId == adminUserId && m.IsAdmin);
                if (adminMembership == null)
                {
                    return new ServiceResponse { Success = false, Message = "Only the group administrator can perform the draw." };
                }

                if (group.IsDrawn)
                {
                    return new ServiceResponse { Success = false, Message = "The draw has already been completed." };
                }

                var members = group.Members.ToList();
                var random = new Random();
                var shuffledMembers = members.OrderBy(_ => random.Next()).ToList();

                for (int i = 0; i < members.Count; i++)
                {
                    var giver = members[i];
                    var receiver = shuffledMembers[(i + 1) % members.Count];
                    giver.AssignedFriendId = receiver.UserId;
                }

                group.IsDrawn = true;

                await _unitOfWork.SaveChangesAsync();

                _logger.LogInformation("Draw completed successfully.");

                return new ServiceResponse { Success = true, Message = "Draw completed successfully." };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error performing draw.");
                return new ServiceResponse { Success = false, Message = "Error performing draw." };
            }
        }

        public async Task<ServiceResponse<List<ValidGroupDto>>> GetAllValidGroupsAsync()
        {
            try
            {
                var validGroups = await _unitOfWork.Groups.GetAllValidAsync();
                if (validGroups == null || !validGroups.Any())
                {
                    return new ServiceResponse<List<ValidGroupDto>>
                    {
                        Success = false,
                        Message = "No valid groups available.",
                        Data = new List<ValidGroupDto>()
                    };
                }

                var groupDtos = validGroups.Select(group => new ValidGroupDto
                {
                    GroupId = group.Id,
                    GroupName = group.Name,
                    Description = group.Description,
                    CreatedAt = group.CreatedAt,
                    CreatedBy = group.CreatedBy.Username
                }).ToList();

                _logger.LogInformation("Valid groups retrieved successfully.");

                return new ServiceResponse<List<ValidGroupDto>>
                {
                    Success = true,
                    Message = "Valid groups retrieved successfully.",
                    Data = groupDtos
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting valid groups.");
                return new ServiceResponse<List<ValidGroupDto>>
                {
                    Success = false,
                    Message = "Error getting valid groups."
                };
            }
        }
    }
}
