using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;

namespace MeetAdl.Pages.Groups
{
    public class DetailsModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly IPostRepository postRepository;
        public Group? GroupDetails;
        public DetailsModel(IGroupRepository groupRepository, IPostRepository postRepository)
        {
            this.groupRepository = groupRepository;
            this.postRepository = postRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId)
        {
            GroupDetails = await groupRepository.GetGroupDetailsAsync(groupId);
            if (GroupDetails == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostDeletePostAsync([FromQuery] long groupId, [FromQuery] long postId)
        {
            await postRepository.DeletePostFromGroupAsync(groupId, postId);
            return await OnGetAsync(groupId);
        }

        public async Task<IActionResult> OnPostDeleteMemberAsync([FromQuery] long groupId, [FromQuery] long userId)
        {
            await groupRepository.DeleteMemberFromGroupAsync(groupId, userId);
            return await OnGetAsync(groupId);
        }

    }
}
