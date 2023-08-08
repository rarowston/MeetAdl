using MeetAdl.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class GroupRepository : IGroupRepository
{
    private readonly MeetAdlDbContext _dbContext;

    public GroupRepository(MeetAdlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Group?> GetGroupSummaryAsync(long id)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<Group?> GetGroupDetailsAsync(long id)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Include(group => group.UserGroups)
            .ThenInclude(userGroup => userGroup.User)
            .Include(group => group.MeetupEvents)
            .Include(group => group.Posts)
            .ThenInclude(post => post.User)
            .Where(group => group.Id == id)
            .FirstOrDefaultAsync();
    }

    public async Task<List<Group>> ListAllGroupsAsync()
    {
        return await _dbContext.Groups.AsNoTracking().ToListAsync();
    }

    public async Task<List<Group>> ListGroupsForUserAsync(long userId)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Include(group => group.UserGroups)
            .Where(group => group.UserGroups != null && group.UserGroups.Where(ug => ug.UserId == userId).Any())
            .ToListAsync();
    }

    public async Task<bool> CheckIfGroupExistsAsync(long groupId)
    {
        return await _dbContext.Groups
            .AsNoTracking()
            .Where(group => group.Id == groupId)
            .AnyAsync();
    }

    public async Task<Group> CreateGroupAsync(string groupName, string groupDescription)
    {
        Group newGroup = new Group()
        {
            Name = groupName,
            Description = groupDescription
        };

        _dbContext.Groups.Add(newGroup);
        await _dbContext.SaveChangesAsync();
        return newGroup;
    }

    public async Task<bool> DeleteMemberFromGroupAsync(long groupId, long userId)
    {
        GroupMember? member = await _dbContext.GroupMembers
            .Where(member => member.GroupId == groupId && member.UserId == userId)
            .FirstOrDefaultAsync();

        if (member == null)
        {
            return false;
        }

        _dbContext.Remove(member);

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateGroupDetailsAsync(long groupId, string groupName, string groupDescription)
    {
        Group? group = await _dbContext.Groups
            .Where(group => group.Id == groupId)
            .FirstOrDefaultAsync();
        if (group == null)
        {
            return false;
        }
        group.Name = groupName;
        group.Description = groupDescription;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<GroupMember?> GetGroupMemberAsync(long groupId, long userId)
    {
        return await _dbContext.GroupMembers
            .AsNoTracking()
            .Where(groupMember => groupMember.GroupId == groupId && groupMember.UserId == userId)
            .FirstOrDefaultAsync();
        throw new NotImplementedException();
    }

    public async Task<Group?> AddOrUpdateUserMembershipForGroupAsync(long groupId, long userId, string? joinMessage)
    {
        GroupMember? membership = await _dbContext.GroupMembers
            .Where(groupMember => groupMember.GroupId == groupId && groupMember.UserId == userId)
            .FirstOrDefaultAsync();

        if (membership != null)
        {
            // Update exisitng details
            membership.JoiningComments = joinMessage;
        }
        else
        {
            // Check if group exists and add user to it
            Group? group = await _dbContext.Groups
                .Include(group => group.UserGroups)
                .Where(group => group.Id == groupId)
                .FirstOrDefaultAsync();

            if (group == null)
            {
                return null;
            }
            group.UserGroups.Add(new GroupMember
            {
                GroupId = groupId,
                JoiningComments = joinMessage,
                UserId = userId
            });
        }

        await _dbContext.SaveChangesAsync();
        return await GetGroupSummaryAsync(groupId);
    }
}