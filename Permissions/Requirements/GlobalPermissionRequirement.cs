using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Permissions.Requirements;
public class GlobalPermissionRequirement : IAuthorizationRequirement
{
    public PermissionLevel PermissionLevel { get; }

    public GlobalPermissionRequirement(PermissionLevel permissionLevel)
    {
        PermissionLevel = permissionLevel;
    }
}
