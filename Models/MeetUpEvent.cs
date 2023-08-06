using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Models;
public class MeetUpEvent
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The name of the event
    /// </summary>
    public string Name { get; set; } = "New Event";

    /// <summary>
    /// A description for the event
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Where the event is located
    /// </summary>
    public string Location { get; set; } = "Unspecified";

    /// <summary>
    /// When the event is being held
    /// </summary>
    public DateTime DateTimeAt { get; set; }

    public long GroupId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Group? Group { get; set; }

    public List<UserEvent> UserEvents { get; set; } = new();
}