using MeetAdl.Models;
using MeetAdl.Permissions;

namespace MeetAdl.Security;
public interface ICurrentIdentityService
{
    /// <summary>
    /// The users object Id from AAD.
    /// Used to match against an internal user record's Object Id (if one exists).
    /// </summary>
    public Guid ObjectId { get; }

    /// <summary>
    /// retrieves the user's information from the database
    /// </summary>
    public Task<User?> GetCurrentUserInformationAsync();

    /// <summary>
    /// Checks whether the current user has the requested permission level
    /// </summary>
    /// <param name="permission">The permission bit(s) to validate</param>
    /// <returns>True if the user has all of the permission bits sent, false otherwise</returns>
    public Task<bool> CurrentUserHasPermissionLevelAsync(PermissionLevel permission);
}

