using MeetAdl.Data;
using MeetAdl.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace MeetAdl.Pages.Groups.Events
{
    public class DetailsModel : PageModel
    {
        private readonly IEventRepository eventRepository;

        public MeetUpEvent? EventDetails;

        public DetailsModel(IEventRepository eventRepository)
        {
            this.eventRepository = eventRepository;
        }

        public async Task<IActionResult> OnGetAsync([FromQuery] long groupId, [FromQuery] long eventId)
        {
            EventDetails = await eventRepository.GetEventDetailsForGroupAsync(groupId, eventId);

            if (EventDetails == null)
            {
                return NotFound();
            }

            return Page();
        }

    }
}
