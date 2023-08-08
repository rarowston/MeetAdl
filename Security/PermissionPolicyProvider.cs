using MeetAdl.Permissions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MeetAdl.Security;

public class PermissionPolicyProvider : DefaultAuthorizationPolicyProvider
{
    private readonly AuthorizationOptions _options;

    public PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        // Check static policies first
        var policy = await base.GetPolicyAsync(policyName);

        // If Policy is not found, try to create a new policy to match the policy name
        if (policy == null)
        {
            // If for whatever reason it is not successful, apply the highest level policy by default. 
            policy = GetAdminPolicy();

            // Check that there is a policy, which should be the global permission level
            if (policyName.Length != 0 && !string.IsNullOrEmpty(policyName))
            {
                // Convert the policy name back to a role requirement
                bool globalPermissionRequirementSuccess = Enum.TryParse(policyName, out PermissionLevel adminPermissionLevel);
                if (globalPermissionRequirementSuccess)
                {
                    // We have at least an admin permission requirement, so set the policy to that.
                    policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddRequirements(new GlobalPermissionRequirement(adminPermissionLevel))
                        .Build();
                }
            }

            // Add policy to the AuthorizationOptions, so we don't have to re-create it each time
            _options.AddPolicy(policyName, policy);
        }

        return policy;
    }

    private static AuthorizationPolicy GetAdminPolicy()
    {
        return new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .AddRequirements(new GlobalPermissionRequirement(PermissionLevel.GlobalAdmin))
            .Build();
    }
}