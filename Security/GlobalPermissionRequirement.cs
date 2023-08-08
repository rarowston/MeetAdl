using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Security;
public class GlobalPermissionRequirement : IAuthorizationRequirement
{
    public PermissionLevel PermissionLevel { get; }

    public GlobalPermissionRequirement(PermissionLevel permissionLevel)
    {
        PermissionLevel = permissionLevel;
    }
}
