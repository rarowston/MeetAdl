using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups
{
    public class UpdateModel : PageModel
    {
        private readonly IGroupRepository groupRepository;

        [BindProperty]
        [Required, MinLength(5)]
        public string? GroupName { get; set; }

        [BindProperty]
        public string? GroupDescription { get; set; }

        [BindProperty]
        public long? GroupId { get; set; }

        public UpdateModel(IGroupRepository groupRepository)
        {
            this.groupRepository = groupRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId)
        {
            Group? group = await groupRepository.GetGroupSummaryAsync(groupId);

            if (group == null)
            {
                return NotFound();
            }

            GroupDescription = group.Description;
            GroupId = group.Id;
            GroupName = group.Name;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId, [FromQuery] long postId)
        {

            if(!ModelState.IsValid || GroupDescription == null || GroupName == null)
            {
                return Page();
            }

            await groupRepository.UpdateGroupDetailsAsync(groupId, GroupName, GroupDescription);

            // Send back to detail page
            return RedirectToPage("./details", new { groupId  = groupId });
        }
    }
}
