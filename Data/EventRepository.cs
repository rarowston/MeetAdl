using MeetAdl.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class EventRepository : IEventRepository
{
    private readonly MeetAdlDbContext _dbContext;

    public EventRepository(MeetAdlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MeetUpEvent> CreateEventForGroupAsync(long groupId, string eventName, string? eventDescription, string eventLocation, DateTime eventDate)
    {
        MeetUpEvent meetUpEvent = new()
        {
            GroupId = groupId,
            Name = eventName,
            Description = eventDescription,
            Location = eventLocation,
            DateTimeAt = eventDate
        };

        _dbContext.Events.Add(meetUpEvent);
        await _dbContext.SaveChangesAsync();
        return meetUpEvent;
    }

    public async Task<MeetUpEvent?> GetEventDetailsForGroupAsync(long groupId, long eventId)
    {
        return await _dbContext.Events
            .AsNoTracking()
            .Include(e => e.Group)
            .Include(e => e.UserEvents)
            .ThenInclude(ue => ue.User)
            .Where(e => e.Id == eventId && e.GroupId == groupId)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> UpdateEventForGroupAsync(long groupId, long eventId, string eventName, string? eventDescription, string eventLocation, DateTime eventDate)
    {
        MeetUpEvent? meetUpEvent = await _dbContext.Events
            .Where(e => e.Id == eventId && e.GroupId == groupId)
            .FirstOrDefaultAsync();

        if (meetUpEvent == null)
        {
            return false;
        }
        meetUpEvent.Name = eventName;
        meetUpEvent.Description = eventDescription;
        meetUpEvent.Location = eventLocation;
        meetUpEvent.DateTimeAt = eventDate;

        await _dbContext.SaveChangesAsync();
        return true;
    }
}