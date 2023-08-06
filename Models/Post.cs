using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Models;

public class Post
{
    [Key]
    public long Id { get; set; }

    /// <summary>
    /// The text content of the post
    /// </summary>
    public string Text { get; set; } = "";

    /// <summary>
    /// The Id of the User who made this post
    /// </summary>
    public long? UserId { get; set; }
    [DeleteBehavior(DeleteBehavior.SetNull)]
    public User? User { get; set; }
    
    /// <summary>
    /// The Id of the post that this group is for
    /// </summary>
    public long GroupId { get; set; }
    [DeleteBehavior(DeleteBehavior.Cascade)]
    public Group? Group { get; set; }
}