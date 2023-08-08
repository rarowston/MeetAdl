using MeetAdl.Security;
using Microsoft.AspNetCore.Authorization;

namespace MeetAdl.Permissions.Requirements;
public class GlobalPermissionHandler : AuthorizationHandler<GlobalPermissionRequirement>
{
    private readonly ICurrentIdentityService _permissionsService;

    public GlobalPermissionHandler(ICurrentIdentityService permissionsService) : base()
    {
        _permissionsService = permissionsService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GlobalPermissionRequirement requirement)
    {
        // If user authentication fails, set context to false and give up, we cannot authorise an unauthenticated user
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return;
        }

        // Check if the user has the required permission level using bitwise operations. 
        // The user's permissions and the required permissions should equal the required permissions if all required bits are set
        // e.g. a bitwise AND of a user permission of 1101 and required permission mask of 0100 would result in 0100 (and thus pass)
        // If the required permission was 1010 a BITwise and would result in 1000 which does not match as the third bit (10x0) was not present in the user's permissions
        if (await _permissionsService.CurrentUserHasPermissionLevelAsync(requirement.PermissionLevel))
        {
            context.Succeed(requirement);
        }
    }

}