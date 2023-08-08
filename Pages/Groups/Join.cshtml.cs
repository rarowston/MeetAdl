using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;

namespace MeetAdl.Pages.Groups
{
    public class JoinModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly ICurrentIdentityService currentIdentityService;

        [BindProperty]
        public Group? Group { get; set; }

        public bool UserIsAlreadyMember = false;

        [BindProperty]
        public string? JoinMessage { get; set; }

        public JoinModel(IGroupRepository groupRepository, ICurrentIdentityService currentIdentityService)
        {
            this.groupRepository = groupRepository;
            this.currentIdentityService = currentIdentityService;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId)
        {
            Group = await groupRepository.GetGroupSummaryAsync(groupId);
            if (Group == null)
            {
                return NotFound();
            }

            User? u = await currentIdentityService.GetCurrentUserInformationAsync();
            if(u == null)
            {
                return Unauthorized();
            }

            GroupMember? membership = await groupRepository.GetGroupMemberAsync(groupId, u.Id);

            if (membership != null)
            {
                UserIsAlreadyMember = true;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId)
        {
            // Apply update
            User? u = await currentIdentityService.GetCurrentUserInformationAsync();
            if (u == null)
            {
                return Unauthorized();
            }

            Group = await groupRepository.AddOrUpdateUserMembershipForGroupAsync(groupId, u.Id, JoinMessage);
            if (Group == null)
            {
                return NotFound();
            }
            
            // Send back to detail page
            return RedirectToPage("details", new { groupId = groupId });
        }
    }
}
