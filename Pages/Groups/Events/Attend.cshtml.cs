using MeetAdl.Data;
using MeetAdl.Models;
using MeetAdl.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace MeetAdl.Pages.Groups.Events
{
    [Authorize]
    public class AttendModel : PageModel
    {
        private readonly ICurrentIdentityService currentIdentityService;
        private readonly IEventRepository eventRepository;

        //[BindProperty]
        //public long? EventId { get; set; }
        //[BindProperty]
        //public long? GroupId { get; set; }

        [BindProperty]
        public string? RsvpDetails { get; set; }

        public MeetUpEvent? EventDetails;

        public AttendModel(ICurrentIdentityService currentIdentityService, IEventRepository eventRepository)
        {
            this.currentIdentityService = currentIdentityService;
            this.eventRepository = eventRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId, [FromQuery] long eventId)
        {
            EventDetails = await eventRepository.GetEventDetailsForGroupAsync(groupId, eventId);

            if (EventDetails == null)
            {
                return NotFound();
            }
            User? u = await currentIdentityService.GetCurrentUserInformationAsync();
            if(u!= null)
            {
                UserEvent? userEvent = await eventRepository.GetUserRsvpToEventAsync(eventId, u.Id);
                RsvpDetails = userEvent?.RsvpDetails;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync([FromQuery] long groupId, [FromQuery] long eventId)
        {
            EventDetails = await eventRepository.GetEventDetailsForGroupAsync(groupId, eventId);

            if (EventDetails == null)
            {
                return NotFound();
            }

            User? u = await currentIdentityService.GetCurrentUserInformationAsync();
            if (u == null)
            {
                return Unauthorized();
            }

            await eventRepository.AddOrUpdateEventRsvpAsync(groupId, eventId, u.Id, RsvpDetails);


            // Send back to detail page
            return RedirectToPage("./details", new { groupId = groupId, eventId = eventId });
        }
    }
}
