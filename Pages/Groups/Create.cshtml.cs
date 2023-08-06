using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups;

public class CreateModel : PageModel
{
    private readonly IGroupRepository groupRepository;


    [BindProperty]
    [Required, MinLength(5)]
    public string? GroupName { get; set; }

    [BindProperty]
    [Required, MinLength(5)]
    public string? GroupDescription { get; set; }

    public CreateModel(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
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

        Group group = await groupRepository.CreateGroupAsync(GroupName, GroupDescription);

        return RedirectToPage("./details", new { groupId = group.Id });

    }

}