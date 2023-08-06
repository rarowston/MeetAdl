using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;

namespace MeetAdl.Pages.Groups
{
    public class JoinModel : PageModel
    {
        private readonly IGroupRepository groupRepository;

        public Group Group;

        [BindProperty]
        public string? JoinMessage { get; set; }

        public JoinModel(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
            Group = new();
        }

        public async Task<IActionResult> OnGetAsync([FromRoute] long groupId)
        {
            Group? foundGroup = await groupRepository.GetGroupDetailsAsync(groupId);
            if (foundGroup == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSaveAsync([FromRoute] long groupId)
        {
            // Add new member
            Group? foundGroup = await groupRepository.AddUserToGroupAsync(groupId, 2, JoinMessage);
            if (foundGroup == null)
            {
                return NotFound();
            }
            
            // Send back to detail page
            return RedirectToPage("details");
        }
    }
}
