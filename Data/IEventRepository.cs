using MeetAdl.Models;

namespace MeetAdl.Data;
public interface IEventRepository
{
    public Task<MeetUpEvent> CreateEventForGroupAsync(long groupId, string eventName, string? eventDescription, string eventLocation, DateTime eventDate);
    public Task<bool> UpdateEventForGroupAsync(long groupId, long eventId, string eventName, string? eventDescription, string eventLocation, DateTime eventDate);
    public Task<MeetUpEvent?> GetEventDetailsForGroupAsync(long groupId, long eventId);
    public Task<UserEvent?> GetUserRsvpToEventAsync(long eventId, long userId);
    public Task<bool> AddOrUpdateEventRsvpAsync(long groupId, long eventId, long userId, string? rsvpDetails);
    public Task<bool> RemoveEventRsvpAsync(long groupId, long eventId, long userId);
}
