using MeetAdl.Security;
using Microsoft.AspNetCore.Authorization;

namespace MeetAdl.Permissions.Requirements;
public class GroupPermissionHandler : AuthorizationHandler<GroupPermissionRequirement>
{
    private readonly ICurrentIdentityService permissionsService;
    private readonly IHttpContextAccessor httpContextAccessor;

    public GroupPermissionHandler(ICurrentIdentityService permissionsService, IHttpContextAccessor httpContextAccessor) : base()
    {
        this.permissionsService = permissionsService;
        this.httpContextAccessor = httpContextAccessor;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, GroupPermissionRequirement requirement)
    {
        // If user authentication fails, set context to false and give up, we cannot authorise an unauthenticated user
        if (context.User?.Identity?.IsAuthenticated != true)
        {
            context.Fail();
            return;
        }

        // If the user does not succeed on a global permission level - check object level
        try
        {
            // Try to get the groupId out of the route
            string? groupIdString = httpContextAccessor.HttpContext?.Request.Query["groupId"].FirstOrDefault();
            long groupId = Convert.ToInt64(groupIdString);
            if (groupId != 0)
            {
                // Check if the user has access to the Group
                if (await permissionsService.CurrentUserCanAccessGroupAsync(groupId, requirement.PermissionLevel))
                {
                    context.Succeed(requirement);
                }
            }
        }
        catch
        {

        }
    }

}