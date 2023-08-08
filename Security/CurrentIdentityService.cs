using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Permissions;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace MeetAdl.Security;
public class CurrentIdentityService : ICurrentIdentityService
{
    private readonly IHttpContextAccessor _accessor;
    private readonly IUserRepository userRepository;

    private Guid _objectId = Guid.Empty;
    private User? _user = null;

    public CurrentIdentityService(IHttpContextAccessor accessor, IUserRepository userRepository)
    {
        _accessor = accessor;
        this.userRepository = userRepository;
    }

    public Guid ObjectId
    {
        get
        {
            // If we have this cached then send it back already for those sweet sweet micro-optimisations
            if (_objectId != Guid.Empty)
            {
                return _objectId;
            }

            // Find the claim for the users auth provider id and set the value if it exists
            Claim? oidClaim = _accessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.ToLower().Equals(ClaimConstants.ObjectId.ToLower()) || c.Type.ToLower().Equals(ClaimConstants.Oid.ToLower()));
            if (oidClaim != null)
            {
                Guid.TryParse(oidClaim.Value, out _objectId);
            }

            return _objectId;
        }
    }

    public async Task<bool> CurrentUserHasPermissionLevelAsync(PermissionLevel permission)
    {
        User? user = await GetCurrentUserInformationAsync();
        if (user?.PermissionLevel == null)
        {
            return false;
        }
        
        return user.PermissionLevel.HasFlag(permission);

        //ALT: return (permission & user.PermissionLevel) == permission;
    }

    public async Task<User?> GetCurrentUserInformationAsync()
    {
        if (_user != null)
        {
            return _user;
        }

        if (ObjectId == Guid.Empty)
        {
            return null;
        }

        _user = await userRepository.GetUserFromObjectIdAsync(ObjectId);

        string? email = null;
        Claim? emailClaim = _accessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.ToLower().Equals(ClaimTypes.Email.ToLower()));
        if (emailClaim != null)
        {
            email = emailClaim.Value;
        }

        // If user does not exist, create the user profile. 
        if (_user == null)
        {
            string displayName = "New User";

            Claim? nameClaim = _accessor.HttpContext?.User?.Claims.FirstOrDefault(c => c.Type.ToLower().Equals(ClaimConstants.Name.ToLower()) || c.Type.ToLower().Equals(ClaimTypes.Name.ToLower()));
            if (nameClaim != null)
            {
                displayName = nameClaim.Value;
            }

            _user = await userRepository.CreateUserRecordAsync(ObjectId, displayName, email);
        }
        else if (_user.Email != email && email != null)
        {
            // Update the email if it doesn't match
            await userRepository.UpdateUserEmailAsync(_user.Id, email);
        }

        return _user;
    }
}

