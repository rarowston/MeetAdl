using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Models;

public class User
{
    [Key]
    public long Id { get; set; }

    public string DisplayName { get; set; } = "Unknown User";

    public string? Email { get; set; }

    public Guid ObjectId { get; set; }

    //public PermissionLevel PermissionLevel { get; set; }

    public bool IsActive { get; set; } = true;

    public IList<GroupMember>? GroupMemberships { get; set; }
}