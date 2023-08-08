using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Identity.Web;
using System.Security.Claims;

namespace MeetAdl.Pages.Profile;

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
            return RedirectToPage("/Index");
        }

        if (userId == null)
        {
            // Retrieve the information about the current user
            ProfileDetails = await currentIdentityService.GetCurrentUserInformationAsync();
        }
        else
        {
            // Retrieve the information about some other user. 
            PageForCurrentUser = false;
            ProfileDetails = await userRepository.GetUserFromUserIdAsync(userId.Value);
        }

        if (ProfileDetails == null)
        {
            return NotFound();
        }

        return Page();

    }
}

