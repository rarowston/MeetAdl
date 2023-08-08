using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
namespace MeetAdl.Permissions.Requirements;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class AuthorizeForGroupAccess : AuthorizeAttribute
{
    public PermissionLevel AdminRole { get; set; }
    public AuthorizeForGroupAccess(PermissionLevel permissionLevel) : base()
    {
        AdminRole = permissionLevel;
        Policy = permissionLevel.ToString() + "," + PermissionConstants.GROUP_PERMISSIONS;
    }
}
