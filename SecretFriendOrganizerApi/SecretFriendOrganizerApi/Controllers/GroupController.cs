using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SecretFriendOrganizer.Application.DTOs;
using SecretFriendOrganizer.Application.Interfaces.Services;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class GroupController : ControllerBase
{
    private readonly IGroupService _groupService;

    public GroupController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    // Endpoint para listar todos os grupos do usuário
    [HttpGet("user-groups/{userId}")]
    public async Task<IActionResult> ListUserGroups(Guid userId)
    {
        var result = await _groupService.ListUserGroupsAsync(userId);
        if (!result.Success)
        {
            return NotFound(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message, Groups = result.Data });
    }

   
    [HttpGet("assigned-friend/{userId}/{groupId}")]
    public async Task<IActionResult> GetAssignedFriend(Guid userId, Guid groupId)
    {
        var result = await _groupService.GetAssignedFriendAsync(userId, groupId);
        if (!result.Success)
        {
            return NotFound(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message, AssignedFriend = result.Data });
    }

    // Criar um grupo
    [HttpPost("create")]
    public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _groupService.CreateGroupAsync(request.GroupName, request.Description, request.CreatorUserId);
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }

    // Realizar o sorteio
    [HttpPost("perform-draw/{groupId}")]
    public async Task<IActionResult> PerformDraw(Guid groupId, [FromBody] PerformDrawRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _groupService.PerformDrawAsync(groupId, request.AdminUserId);
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }

    [HttpPost("join-group")]
    public async Task<IActionResult> JoinGroup([FromBody] JoinGroupRequest request)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var result = await _groupService.AddUserToGroupAsync(request.GroupId, request.UserId, request.GiftRecommendation);
        if (!result.Success)
        {
            return BadRequest(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message });
    }


    [HttpGet("valid-groups")]
    public async Task<IActionResult> GetAllValidGroups()
    {
        var result = await _groupService.GetAllValidGroupsAsync();
        if (!result.Success)
        {
            return NotFound(new { Message = result.Message });
        }

        return Ok(new { Message = result.Message, Groups = result.Data });
    }
}
