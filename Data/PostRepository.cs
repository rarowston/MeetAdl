using MeetAdl.Models;
using Microsoft.EntityFrameworkCore;

namespace MeetAdl.Data;

public class PostRepository : IPostRepository
{
    private readonly MeetAdlDbContext _dbContext;

    public PostRepository(MeetAdlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task AddPostForGroupAsync(long groupId, long userId, string postContent)
    {
        _dbContext.Posts.Add(new Post
        {
            GroupId = groupId,
            Text = postContent,
            UserId = userId
        });
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> DeletePostFromGroupAsync(long groupId, long postId)
    {
        Post? post = await _dbContext.Posts
            .Where(post => post.GroupId == groupId && post.Id == postId)
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return false;
        }

        _dbContext.Remove(post);

        await _dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<Post?> GetPostForGroupAsync(long groupId, long postId)
    {
        return await _dbContext.Posts
            .AsNoTracking()
            .Where(post => post.GroupId == groupId && post.Id == postId)
            .FirstOrDefaultAsync();
    }

    public async Task<Post?> UpdatePostContentForGroupAsync(long groupId, long postId, string postContent)
    {
        Post? post = await _dbContext.Posts
            .Where(post => post.GroupId == groupId && post.Id == postId)
            .FirstOrDefaultAsync();

        if (post == null)
        {
            return null;
        }

        post.Text = postContent;

        await _dbContext.SaveChangesAsync();
        return post;
    }
}