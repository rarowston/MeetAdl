using MeetAdl.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class MeetAdlDbContext : DbContext
{
    public MeetAdlDbContext()
    {

    }

    public DbSet<MeetUpEvent> Events { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupMember> GroupMembers { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserEvent> UserEvents { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        // connect to sql server with connection string from app settings
        options.UseSqlServer("Server=M-Prime-Ryan\\DEMONSTRATION;Database=MeetAdl;Trusted_Connection=True;Timeout=180;ConnectRetryCount=3;ConnectRetryInterval=10;TrustServerCertificate=True");
    }

}