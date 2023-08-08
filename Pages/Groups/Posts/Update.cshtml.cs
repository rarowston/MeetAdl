using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups.Posts
{
    public class UpdateModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly IPostRepository postRepository;

        public Group? GroupDetails;
        public Post? PostDetails;

        [BindProperty]
        [Required, MinLength(5)]
        public string? PostContent { get; set; }

        public UpdateModel(IGroupRepository groupRepository, IPostRepository postRepository)
        {
            this.groupRepository = groupRepository;
            this.postRepository = postRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId, [FromQuery] int? postId)
        {
            GroupDetails = await groupRepository.GetGroupDetailsAsync(groupId);

            if (GroupDetails == null)
            {
                return NotFound();
            }

            if (postId != null && postId != 0)
            {
                PostDetails = await postRepository.GetPostForGroupAsync(groupId, postId.Value);
            }

            if (PostDetails == null)
            {
                return NotFound();
            }

            PostContent = PostDetails.Text;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId, [FromQuery] long? postId)
        {
            GroupDetails = await groupRepository.GetGroupDetailsAsync(groupId);
            if (GroupDetails == null)
            {
                return NotFound();
            }

            if (postId != null && postId != 0)
            {
                if (PostContent == null)
                {
                    PostDetails = await postRepository.GetPostForGroupAsync(groupId, postId.Value);
                    return Page();
                }
                PostDetails = await postRepository.UpdatePostContentForGroupAsync(groupId, postId.Value, PostContent);
            }
            else
            {
                return NotFound();
            }

            // Send back to detail page
            return RedirectToPage("../details", new { groupId  = groupId });
        }
    }
}
