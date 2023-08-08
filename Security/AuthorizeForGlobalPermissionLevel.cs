using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Security;

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
