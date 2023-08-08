using MeetAdl.Permissions;
using MeetAdl.Permissions.Requirements;
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

            // Split out the parts of a policy to apply. These are separated by commas.
            string[] policies = policyName.Split(',');

            // Check that there is a policy, which should be the global permission level
            if (policies.Length != 0 && !string.IsNullOrEmpty(policies[0]))
            {
                // Convert the policy name back to a role requirement
                bool globalPermissionRequirementConversionSuccess = Enum.TryParse(policies[0], out PermissionLevel adminPermissionLevel);
                if (globalPermissionRequirementConversionSuccess)
                {
                    // We have at least an admin permission requirement, so set the policy to that.
                    policy = new AuthorizationPolicyBuilder()
                        .RequireAuthenticatedUser()
                        .AddRequirements(new GlobalPermissionRequirement(adminPermissionLevel))
                        .Build();

                    // Check that there is a second policy, which should be the object specific option (if any)
                    if (policies.Length > 1 && !string.IsNullOrEmpty(policies[1]))
                    {
                        // There is a second Policy in the array - Check if it is a type that we can process
                        switch (policies[1])
                        {
                            case PermissionConstants.GROUP_PERMISSIONS:
                                // There is a campaign requirement, so set a campaign permission requirement
                                policy = new AuthorizationPolicyBuilder()
                                    .RequireAuthenticatedUser()
                                    .AddRequirements(new GroupPermissionRequirement(adminPermissionLevel))
                                    .Build();
                                break;
                        }

                    }
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