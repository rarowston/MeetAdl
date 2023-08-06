using System.ComponentModel.DataAnnotations;
using MeetAdl.Models.Enums;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Models;

public class UserEvent
{
    [Key]
    public long Id { get; set; }

    public string? RsvpDetails { get; set; }

    public Rating? Rating { get; set; }
    public string? Feedback { get; set; }

    public long UserId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }

    public long EventId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public MeetUpEvent? Event { get; set; }
}