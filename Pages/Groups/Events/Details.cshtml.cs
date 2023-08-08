using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MeetAdl.Pages.Groups.Events;

[AllowAnonymous]
public class DetailsModel : PageModel
{
    private readonly IEventRepository eventRepository;
    private readonly ICurrentIdentityService currentIdentityService;
    public MeetUpEvent? EventDetails;
    public bool CurrentUserGoing = false;

    public DetailsModel(IEventRepository eventRepository, ICurrentIdentityService currentIdentityService)
    {
        this.eventRepository = eventRepository;
        this.currentIdentityService = currentIdentityService;
    }

    public async Task<IActionResult> OnGetAsync([FromQuery] long groupId, [FromQuery] long eventId)
    {
        EventDetails = await eventRepository.GetEventDetailsForGroupAsync(groupId, eventId);

        if (EventDetails == null)
        {
            return NotFound();
        }

        User? u = await currentIdentityService.GetCurrentUserInformationAsync();
        if (u != null)
        {
            UserEvent? userEvent = await eventRepository.GetUserRsvpToEventAsync(eventId, u.Id);

            if (userEvent != null)
            {
                CurrentUserGoing = true;
            }
        }

        return Page();
    }

    public async Task<IActionResult> OnPostCancelRsvpAsync([FromQuery] long groupId, [FromQuery] long eventId)
    {
        User? u = await currentIdentityService.GetCurrentUserInformationAsync();
        if (u != null)
        {
            await eventRepository.RemoveEventRsvpAsync(groupId, eventId, u.Id);
        }

        return RedirectToPage("./Details", new { groupId = groupId, eventId = eventId });
    }

}

