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

    public async Task<bool> AddOrUpdateEventRsvpAsync(long groupId, long eventId, long userId, string? rsvpDetails)
    {
        // Validate that the event exists for the group
        MeetUpEvent? meetUpEvent = await _dbContext.Events
            .AsNoTracking()
            .Where(e => e.Id == eventId && e.GroupId == groupId)
            .FirstOrDefaultAsync();
        if (meetUpEvent == null)
        {
            return false;
        }

        UserEvent? existingRsvp = await _dbContext.UserEvents
            .Where(userEvent => userEvent.EventId == eventId && userEvent.UserId == userId)
            .FirstOrDefaultAsync();

        if (existingRsvp != null)
        {
            // Update exisitng RSVP
            existingRsvp.RsvpDetails = rsvpDetails;
        }
        else
        {
            _dbContext.UserEvents.Add(new UserEvent
            {
                EventId = eventId,
                UserId = userId,
                RsvpDetails = rsvpDetails
            });
        }

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> RemoveEventRsvpAsync(long groupId, long eventId, long userId)
    {
        // Validate that the event exists for the group
        MeetUpEvent? meetUpEvent = await _dbContext.Events
            .AsNoTracking()
            .Where(e => e.Id == eventId && e.GroupId == groupId)
            .FirstOrDefaultAsync();
        if (meetUpEvent == null)
        {
            return false;
        }

        UserEvent? existingRsvp = await _dbContext.UserEvents
            .Where(userEvent => userEvent.EventId == eventId && userEvent.UserId == userId)
            .FirstOrDefaultAsync();

        if (existingRsvp != null)
        {
            _dbContext.UserEvents.Remove(existingRsvp);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        return false;
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

    public async Task<UserEvent?> GetUserRsvpToEventAsync(long eventId, long userId)
    {
        return await _dbContext.UserEvents
            .AsNoTracking()
            .Where(userEvent => userEvent.EventId == eventId && userEvent.UserId == userId)
            .FirstOrDefaultAsync();
    }
}