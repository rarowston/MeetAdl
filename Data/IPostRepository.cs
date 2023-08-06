using MeetAdl.Models;

namespace MeetAdl.Data;

public interface IPostRepository
{
    public Task AddPostForGroupAsync(long groupId, long userId, string postContent);
    public Task<bool> DeletePostFromGroupAsync(long groupId, long postId);
    public Task<Post?> GetPostForGroupAsync(long groupId, long postId);
    public Task<Post?> UpdatePostContentForGroupAsync(long groupId, long postId, string postContent);
}