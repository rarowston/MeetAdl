using MeetAdl.Models;

namespace MeetAdl.Data;

public interface IUserRepository
{
    public Task<User?> CreateUserRecordAsync(Guid objectId, string displayName, string? email);
    public Task<User?> GetUserFromObjectIdAsync(Guid objectId);
    public Task<User?> GetUserFromUserIdAsync(long userId);
    public Task<bool> UpdateUserEmailAsync(long userId, string email);
}