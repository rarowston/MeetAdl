using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Permissions.Requirements;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeForGlobalPermissionLevel : AuthorizeAttribute
{
    public PermissionLevel AdminRole { get; set; }
    public AuthorizeForGlobalPermissionLevel(PermissionLevel permissionLevel) : base()
    {
        AdminRole = permissionLevel;
        Policy = permissionLevel.ToString();
    }
}
