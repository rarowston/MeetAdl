using MeetAdl.Models;

namespace MeetAdl.Data;

public interface IGroupRepository
{
    public Task<Group?> GetGroupSummaryAsync(long id);
    public Task<Group?> GetGroupDetailsAsync(long id);
    public Task<GroupMember?> GetGroupMemberAsync(long groupId, long userId);
    public Task<Group?> AddOrUpdateUserMembershipForGroupAsync(long groupId, long userId, string? JoinMessage);
    public Task<List<Group>> ListAllGroupsAsync();
    public Task<List<Group>> ListGroupsForUserAsync(long userId);

    public Task<bool> CheckIfGroupExistsAsync(long groupId);
    public Task<Group> CreateGroupAsync(string groupName, string groupDescription);
    public Task<bool> DeleteMemberFromGroupAsync(long groupId, long userId);
    public Task<bool> UpdateGroupDetailsAsync(long groupId, string groupName, string groupDescription);
}