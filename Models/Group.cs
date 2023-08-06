using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Models;

public class Group
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The name of the meetup group
    /// </summary>
    public string Name { get; set; } = "Name Unspecified";
    
    /// <summary>
    /// The description of the meetup group
    /// </summary>
    public string Description { get; set; } = string.Empty;

    public IList<GroupMember> UserGroups { get; set; } = new List<GroupMember>();

    public IList<MeetUpEvent>? MeetupEvents { get; set; }
    public IList<Post> Posts { get; set; } = new List<Post>();
}