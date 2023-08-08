using MeetAdl.Models;

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
}

