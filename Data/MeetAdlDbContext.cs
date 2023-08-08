using MeetAdl.Configuration;
using MeetAdl.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class MeetAdlDbContext : DbContext
{
    private readonly DatabaseConfiguration dbconfig;

    public MeetAdlDbContext(DatabaseConfiguration dbconfig)
    {
        this.dbconfig = dbconfig;
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
        options.UseSqlServer(dbconfig.MeetAdlDatabase);
    }

}