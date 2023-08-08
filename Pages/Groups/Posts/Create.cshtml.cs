using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MeetAdl.Data;
using MeetAdl.Models;
using System.ComponentModel.DataAnnotations;
using MeetAdl.Security;
using MeetAdl.Permissions;

namespace MeetAdl.Pages.Groups.Posts
{
    [AuthorizeForGlobalPermissionLevel(PermissionLevel.CreateGroupPosts)]
    public class CreateModel : PageModel
    {
        private readonly IGroupRepository groupRepository;
        private readonly IPostRepository postRepository;
        private readonly ICurrentIdentityService currentIdentityService;

        public Group? GroupDetails;

        [BindProperty]
        [Required, MinLength(5)]
        public string? PostContent { get; set; }

        public CreateModel(IGroupRepository groupRepository, IPostRepository postRepository, ICurrentIdentityService currentIdentityService)
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

            User? u = await currentIdentityService.GetCurrentUserInformationAsync();
            if (u == null)
            {
                return Unauthorized();
            }

            await postRepository.AddPostForGroupAsync(groupId, u.Id, PostContent);
            
            // Send back to detail page
            return RedirectToPage("../details", new { groupId = groupId });
        }
    }
}
