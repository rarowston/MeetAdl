using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Permissions.Requirements;
public class GroupPermissionRequirement : IAuthorizationRequirement
{
    public PermissionLevel PermissionLevel { get; }

    public GroupPermissionRequirement(PermissionLevel permissionLevel)
    {
        PermissionLevel = permissionLevel;
    }
}
