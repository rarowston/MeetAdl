using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MeetAdl.Pages.Groups;

public class IndexModel : PageModel
{
    private readonly IGroupRepository groupRepository;

    public IndexModel(IGroupRepository groupRepository)
    {
        this.groupRepository = groupRepository;
    }

    [ViewData]
    public IEnumerable<Group>? Groups { get; set; }

    public async Task OnGetAsync()
    {
        Groups = await groupRepository.ListAllGroupsAsync();
    }
}