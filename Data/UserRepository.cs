using MeetAdl.Models;
using MeetAdl.Permissions;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class UserRepository : IUserRepository
{
    private readonly MeetAdlDbContext _dbContext;

    public UserRepository(MeetAdlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<User?> CreateUserRecordAsync(Guid objectId, string displayName, string? email)
    {
        User user = new()
        {
            ObjectId = objectId,
            DisplayName = displayName,
            Email = email,
            IsActive = true
        };

        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task<User?> GetUserFromObjectIdAsync(Guid objectId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => user.ObjectId == objectId)
            .FirstOrDefaultAsync();
    }

    public async Task<User?> GetUserFromUserIdAsync(long userId)
    {
        return await _dbContext.Users
            .AsNoTracking()
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateUserEmailAsync(long userId, string email)
    {
        User? user = await _dbContext.Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
        user.Email = email;
        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserPermissionsAsync(long userId, PermissionLevel permissionLevel)
    {
        User? user = await _dbContext.Users
            .Where(user => user.Id == userId)
            .FirstOrDefaultAsync();
        if (user == null)
        {
            return false;
        }
        user.PermissionLevel = permissionLevel;
        await _dbContext.SaveChangesAsync();
        return true;
    }
}