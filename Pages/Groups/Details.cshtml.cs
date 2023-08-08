using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Authorization;
using MeetAdl.Security;

namespace MeetAdl.Pages.Groups;

[AllowAnonymous]
public class DetailsModel : PageModel
{
    private readonly IGroupRepository groupRepository;
    private readonly IPostRepository postRepository;
    private readonly ICurrentIdentityService currentIdentityService;

    public bool UserIsAlreadyMember = false;
    public Group? GroupDetails;

    public DetailsModel(IGroupRepository groupRepository, IPostRepository postRepository, ICurrentIdentityService currentIdentityService)
    {
        this.groupRepository = groupRepository;
        this.postRepository = postRepository;
        this.currentIdentityService = currentIdentityService;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] long groupId)
    {
        GroupDetails = await groupRepository.GetGroupDetailsAsync(groupId);
        if (GroupDetails == null)
        {
            return NotFound();
        }

        User? u = await currentIdentityService.GetCurrentUserInformationAsync();
        if (u != null)
        {
            GroupMember? membership = await groupRepository.GetGroupMemberAsync(groupId, u.Id);

            if (membership != null)
            {
                UserIsAlreadyMember = true;
            }
        }


        return Page();
    }

    public async Task<IActionResult> OnPostDeletePostAsync([FromQuery] long groupId, [FromQuery] long postId)
    {
        await postRepository.DeletePostFromGroupAsync(groupId, postId);
        return RedirectToPage("details", new { groupId = groupId });
    }

    public async Task<IActionResult> OnPostDeleteMemberAsync([FromQuery] long groupId, [FromQuery] long userId)
    {
        await groupRepository.DeleteMemberFromGroupAsync(groupId, userId);
        return RedirectToPage("details", new { groupId = groupId });
    }

    public async Task<IActionResult> OnPostLeaveGroupAsync([FromQuery] long groupId)
    {
        User? u = await currentIdentityService.GetCurrentUserInformationAsync();
        if(u != null)
        {
            await groupRepository.DeleteMemberFromGroupAsync(groupId, u.Id);
        }
        return RedirectToPage("details", new { groupId = groupId });
    }

}

