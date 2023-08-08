using MeetAdl.Models;
using MeetAdl.Permissions;

namespace MeetAdl.Data;

public interface IUserRepository
{
    public Task<User?> CreateUserRecordAsync(Guid objectId, string displayName, string? email);
    public Task<GroupMember?> GetUserAccessToGroupAsync(long userId, long groupId);
    public Task<User?> GetUserFromObjectIdAsync(Guid objectId);
    public Task<User?> GetUserFromUserIdAsync(long userId);
    public Task<bool> UpdateGroupMembershipPermissionsAsync(long groupId, long userId, PermissionLevel permissionLevel);
    public Task<bool> UpdateUserEmailAsync(long userId, string email);
    public Task<bool> UpdateUserPermissionsAsync(long userId, PermissionLevel permissionLevel);
}