using System.ComponentModel.DataAnnotations;
using MeetAdl.Permissions;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Models;
public class GroupMember
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// Permissions scoped to this specific group for this specific user
    /// </summary>
   // public PermissionLevel UserGroupPermissions { get; set; }

    public string? JoiningComments { get; set; }

    public long UserId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public User? User { get; set; }

    public long GroupId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Group? Group { get; set; }
}