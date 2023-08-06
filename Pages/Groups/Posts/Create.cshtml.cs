using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups.Posts
{
    public class CreateModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly IPostRepository postRepository;

        public Group? GroupDetails;

        [BindProperty]
        [Required, MinLength(5)]
        public string? PostContent { get; set; }

        public CreateModel(IGroupRepository groupRepository, IPostRepository postRepository)
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

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId)
        {
            if (PostContent == null)
            {
                return Page();
            }

            bool groupFound = await groupRepository.CheckIfGroupExistsAsync(groupId);
            if (!groupFound)
            {
                return NotFound();
            }
            
            await postRepository.AddPostForGroupAsync(groupId, 2, PostContent);
            
            // Send back to detail page
            return RedirectToPage("../details", new { groupId = groupId });
        }
    }
}
