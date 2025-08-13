using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups;

[Authorize]
public class CreateModel : PageModel
{
    private readonly IGroupRepository groupRepository;
    private readonly IUserRepository userRepository;
    private readonly ICurrentIdentityService currentIdentityService;

    [BindProperty]
    [Required, MinLength(5)]
    public string? GroupName { get; set; }

    [BindProperty]
    [Required, MinLength(5)]
    public string? GroupDescription { get; set; }

    public CreateModel(IGroupRepository groupRepository, IUserRepository userRepository, ICurrentIdentityService currentIdentityService)
    {
        this.groupRepository = groupRepository;
        this.userRepository = userRepository;
        this.currentIdentityService = currentIdentityService;
    }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid || GroupName == null || GroupDescription == null)
        {
            return Page();
        }

        User? user = await currentIdentityService.GetCurrentUserInformationAsync();
        if (user is null) {
            return Forbid();
        }

        Group group = await groupRepository.CreateGroupAsync(GroupName, GroupDescription);
        await groupRepository.AddOrUpdateUserMembershipForGroupAsync(group.Id, user.Id, "Founder");
        bool success = await userRepository.UpdateGroupMembershipPermissionsAsync(group.Id, user.Id, Permissions.PermissionLevel.GroupAdministrate);
        
        return RedirectToPage("./details", new { groupId = group.Id });

    }

}