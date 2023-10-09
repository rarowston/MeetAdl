using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Permissions.Requirements;
using MeetAdl.Permissions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MeetAdl.Pages.Profiles;

[AuthorizeForGlobalPermissionLevel(PermissionLevel.ReadUserDetails)]
public class ListModel : PageModel
{
    private readonly ILogger<ListModel> _logger;
    private readonly IUserRepository userRepository;

    public List<User> Users { get; set; } = new();

    public ListModel(ILogger<ListModel> logger, IUserRepository userRepository)
    {
        _logger = logger;
        this.userRepository = userRepository;
    }

    public async Task<IActionResult> OnGetAsync()
    {
        Users = await userRepository.GetAllUsersAsync();

        return Page();

    }
}

