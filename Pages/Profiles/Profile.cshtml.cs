using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace MeetAdl.Pages.Profiles;

public class ProfileModel : PageModel
{
    private readonly ILogger<ProfileModel> _logger;
    private readonly ICurrentIdentityService currentIdentityService;
    private readonly IUserRepository userRepository;

    public User? ProfileDetails { get; set; }

    public bool PageForCurrentUser { get; set; } = true;

    public ProfileModel(ILogger<ProfileModel> logger, ICurrentIdentityService currentIdentityService, IUserRepository userRepository)
    {
        _logger = logger;
        this.currentIdentityService = currentIdentityService;
        this.userRepository = userRepository;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] long? userId)
    {
        if (User.Identity == null || !User.Identity.IsAuthenticated)
        {
            return RedirectToPage("~/Index");
        }

        if (userId == null)
        {
            // Retrieve the information about the current user
            ProfileDetails = await currentIdentityService.GetCurrentUserInformationAsync();
            PageForCurrentUser = true;
        }
        else
        {
            // Retrieve the information about any user. 
            ProfileDetails = await userRepository.GetUserFromUserIdAsync(userId.Value);

            // Check if it is still for the current user
            if (currentIdentityService.ObjectId == (ProfileDetails?.ObjectId))
            {
                PageForCurrentUser = true;
            }
            else
            {
                PageForCurrentUser = false;
            }

        }

        if (ProfileDetails == null)
        {
            return NotFound();
        }

        return Page();

    }
}

